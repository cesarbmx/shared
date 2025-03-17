using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CesarBmx.Shared.Settings;
using CesarBmx.Shared.Application.Responses;
using ErrorMessage = CesarBmx.Shared.Application.Messages.ErrorMessage;
using System;
using System.Linq;

namespace CesarBmx.Shared.Api.ActionFilters
{
    [AttributeUsage(AttributeTargets.All)]
    public class ApiKeyAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly AuthenticationSettings _authenticationSettings;

        public ApiKeyAuthorizationAttribute(AuthenticationSettings authenticationSettings)
        {
            _authenticationSettings = authenticationSettings;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var apiKey = context.HttpContext.Request.Headers["X-ApiKey"].FirstOrDefault();

            if (string.IsNullOrEmpty(apiKey) || apiKey != _authenticationSettings.ApiKey)
            {
                var errorResponse = new Unauthorized(ErrorMessage.Unauthorized);
                context.Result = new UnauthorizedObjectResult(errorResponse);
            }
        }
    }
}