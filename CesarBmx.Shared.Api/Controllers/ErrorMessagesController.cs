using System.Collections.Generic;
using CesarBmx.Shared.Api.ActionFilters;
using CesarBmx.Shared.Application.ResponseBuilders;
using CesarBmx.Shared.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CesarBmx.Shared.Api.Controllers
{
    [SwaggerResponse(500, Type = typeof(InternalServerError))]
    [SwaggerResponse(401, Type = typeof(Unauthorized))]
    [SwaggerResponse(403, Type = typeof(Forbidden))]
    [AllowAnonymous]
    [SwaggerOrder(orderPrefix: "Y")]
    public class ErrorMessagesController : Controller
    {
        /// <summary>
        /// Get error messages
        /// </summary>
        [HttpGet]
        [Route("api/error-messages")]
        [SwaggerResponse(200, Type = typeof(Dictionary<string,string[]>))]
        [SwaggerOperation(Tags = new[] { "Error messages" }, OperationId = "ErrorMessages_GetErrorMessages")]
        public IActionResult GetErrorMessages()
        {
            // Get error messages
            var errorMessages = ErrorMessageResponseBuilder.BuildErrorMessages();
            
            // Return
            return Ok(errorMessages);
        }
    }
}

