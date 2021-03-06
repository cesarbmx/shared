﻿using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Shared.Application.Messages;
using CesarBmx.Shared.Authentication.Helpers;

namespace CesarBmx.Shared.Api.ActionFilters
{
    public class IdentityFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Do nothing if there is no 'request' parameter
            if (!filterContext.ActionArguments.ContainsKey("request")) return;
            // Grab the request object
            var request = filterContext.ActionArguments["request"];
            // Grab the identity
            var identity = filterContext.HttpContext.User;
            // Request must be authenticated 
            if (identity == null) throw new UnauthorizedException(ErrorMessage.Unauthorized);
            // Set the identity values
            IdentityHelper.SetIdentityValues(ref request, identity.Claims.ToList());
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}