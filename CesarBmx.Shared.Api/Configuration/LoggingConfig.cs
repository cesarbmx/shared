using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class LoggingConfig
    {
        public static IServiceCollection ConfigureSharedLogging(this IServiceCollection services)
        {
            services.AddLogging(logging =>
            {
                logging.AddFilter("Default", LogLevel.Information);
                logging.AddFilter("System", LogLevel.None);
                logging.AddFilter("Microsoft", LogLevel.None);
                logging.AddFilter("Hangfire", LogLevel.None);
            });

            return services;
        }
    }
}
