using System;
using Microsoft.Extensions.Logging;

namespace CesarBmx.Shared.Logging.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogRequestInformation<TRequest>(this ILogger logger, TRequest request, object response, string customerId) where TRequest : class
        {
            logger.LogInformation("{@Event}, {@Id}, {@Request}, {@Response}, {@CustomerId}", request.GetType().Name, Guid.NewGuid(), request, response, customerId);
        }
        public static void LogJobInformation<TJob>(this ILogger logger, TJob job, int count, int executionTime)
        {
            logger.LogInformation("{@Event}, {@Id}, {@Count}, {@ExecutionTime}", job.GetType().Name, Guid.NewGuid(), count, executionTime);
        }
    }
}