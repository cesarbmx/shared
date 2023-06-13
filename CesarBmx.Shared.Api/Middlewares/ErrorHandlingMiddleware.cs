using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Shared.Application.Responses;
using ErrorMessage = CesarBmx.Shared.Application.Messages.ErrorMessage;

namespace CesarBmx.Shared.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Response
            object errorResponse;
            int code;
            switch (exception)
            {
                case UnauthorizedException _: // 401
                    var unauthorizedException = exception as UnauthorizedException;
                    code = 401;
                    errorResponse = new Unauthorized( unauthorizedException.Message);
                    break;
                case ForbiddenException _:    // 403
                    var forbiddenException = exception as ForbiddenException;
                    code = 403;
                    errorResponse = new Forbidden(forbiddenException.Message);
                    break;
                case NotFoundException _:     // 404
                    var notFoundException = exception as NotFoundException;
                    code = 404;
                    errorResponse = new NotFound(notFoundException.Message);
                    break;
                case ConflictException _:     // 409
                    var conflictException = exception as ConflictException;
                    code = 409;
                    errorResponse = conflictException.Conflict;
                    break;
                default:                      // 500
                    code = 500;
                    var id = Guid.NewGuid();
                    errorResponse = new InternalServerError(ErrorMessage.InternalServerError, id);
                    var request = context.Items["Request"];
                    if (request != null)
                    {
                        // Log error with request
                        _logger.LogError(exception, "{@Id}, {@Message}, {@Request}", id, exception.Message, request);
                    }
                    else
                    {
                        // Log error
                        _logger.LogError(exception, ", {@Id}, {@Message}", id, exception.Message);
                    }
                    break;
            }

            var response = JsonConvert.SerializeObject(errorResponse, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() }
                });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(response);
        }
    }
}
