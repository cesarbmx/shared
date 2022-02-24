using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class LoggingConfig
    {
        public static IHostBuilder ConfigureSharedLogging(this IHostBuilder hostBuilder)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationId", "Hola")
                .WriteTo.File(new JsonFormatter(), 
                    "Logs/Log.txt",
                    rollingInterval: RollingInterval.Day, 
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Console()
                .MinimumLevel.Override("Default", LogEventLevel.Information)
                .Filter.ByExcluding(Matching.FromSource("System"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Filter.ByExcluding(Matching.FromSource("Hangfire"))
                .Filter.ByExcluding(Matching.FromSource("Default"))
                .CreateLogger();

            hostBuilder.UseSerilog();

            hostBuilder.ConfigureLogging(logging =>
            {
                logging.AddFilter("Default", LogLevel.Information);
                logging.AddFilter("System", LogLevel.None);
                logging.AddFilter("Microsoft", LogLevel.None);
                logging.AddFilter("Hangfire", LogLevel.None);
            });

            return hostBuilder;
        }
        public static ILoggingBuilder ConfigureSharedLogging(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddFilter("Default", LogLevel.Information);
            loggingBuilder.AddFilter("System", LogLevel.None);
            loggingBuilder.AddFilter("Microsoft", LogLevel.None);
            loggingBuilder.AddFilter("Hangfire", LogLevel.None);


            return loggingBuilder;
        }
    }
}
