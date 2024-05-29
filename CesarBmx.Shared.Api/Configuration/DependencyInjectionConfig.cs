using System.Diagnostics;
using CesarBmx.Shared.Api.ActionFilters;
using CesarBmx.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddLogExecutionTime(this IServiceCollection services)
        {
            // Log execution time
            services.AddScoped<LogExecutionTimeAttribute>();

            return services;
        }
        public static IServiceCollection AddActivitySource(this IServiceCollection services, IConfiguration configuration)
        {
            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();

            // Open telemetry
            services.AddSingleton(x => new ActivitySource(appSettings.ApplicationId));

            return services;
        }
       
    }
}
