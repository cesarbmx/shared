﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace CesarBmx.Shared.Api.ActionFilters
{
    public class LogHttpRequestAttribute : ActionFilterAttribute
    {
        private readonly ILogger<LogHttpRequestAttribute> _logger;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public LogHttpRequestAttribute(ILogger<LogHttpRequestAttribute> logger)
        {
            this._logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch.Start();
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
            var actionName = context.HttpContext.GetRouteData().Values["action"]?.ToString();
            _stopwatch.Stop();
            // Log
            _logger.LogInformation("{@Event}, {@Action}, {@ExecutionTime}", "ExecutionTime", actionName, _stopwatch.Elapsed.TotalSeconds);
        }
    }
}