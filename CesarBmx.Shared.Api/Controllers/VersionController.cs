﻿using System.Reflection;
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
    [SwaggerOrder(orderPrefix: "Z")]
    public class VersionController : Controller
    {
        /// <summary>
        /// Get version
        /// </summary>
        [HttpGet]
        [Route("api/version")]
        [SwaggerResponse(200, Type = typeof(Version))]
        [SwaggerOperation(Tags = new[] { "Version" }, OperationId = "Version_GetVersion")]
        public IActionResult GetVersion()
        {
            // Response
            var response = new Version();

            // Build
            response.Build(Assembly.GetEntryAssembly());
                
            // Return
            return Ok(response);
        }
    }
}

