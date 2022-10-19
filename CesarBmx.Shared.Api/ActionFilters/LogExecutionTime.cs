using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace CesarBmx.Shared.Api.ActionFilters
{
    public class LogExecutionTimeAttribute : ActionFilterAttribute
    {
        private readonly ILogger<LogExecutionTimeAttribute> _logger;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public LogExecutionTimeAttribute(ILogger<LogExecutionTimeAttribute> logger)
        {
            _logger = logger;
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
            _logger.LogInformation("{@Event}, {@Id}, {@Action}, {@ExecutionTime}", "ExecutionTime", Guid.NewGuid(), actionName, _stopwatch.Elapsed.TotalSeconds);
        }
    }
}