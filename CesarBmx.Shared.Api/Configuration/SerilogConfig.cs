using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CesarBmx.Shared.Settings;
using CesarBmx.Shared.Common.Extensions;
using Serilog;
using Serilog.Templates;
using System.Reflection;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using Serilog.Sinks.OpenTelemetry;
using Serilog.Filters;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class SerilogConfig
    {
        public static void ConfigureSharedSerilog(this ILoggerFactory loggerFactory, IConfiguration configuration, Assembly assembly)
        {
            // Grab AppSettings
            var appSettings = configuration.GetSection<AppSettings>();

            var environmentSettings = configuration.GetSection<EnvironmentSettings>();

            var loggingSettings = configuration.GetSection<LoggingSettings>();

            var elkSettings = configuration.GetSection<ElkSettings>();

            var sinkOptions = new ElasticsearchSinkOptions(new Uri(elkSettings.ElasticsearchUrl))
            {
                AutoRegisterTemplate = true,
                ModifyConnectionSettings = x => x.GlobalHeaders(new NameValueCollection { { "Authorization", $"Bearer {elkSettings.BearerToken}" } }),
                IndexFormat = $"{environmentSettings.Name}-{appSettings.ApplicationId}-{DateTime.Now:yyyy-MM}",
                CustomFormatter = new ExpressionTemplate(
                            "{ { ..@p, Timestamp: @t, Level: @l, Exception: @x, SourceContext: undefined(), ActionId: undefined() } }\r\n")
            };

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()

                // Common properties
                .Enrich.WithProperty("Team", "CustomerTeam")
                .Enrich.WithProperty("App", appSettings.ApplicationId)
                .Enrich.WithProperty("Version", assembly.VersionNumber())
                .Enrich.WithProperty("Environment", environmentSettings.Name)

                // Exclude everything else 
                .Filter.ByExcluding(Matching.FromSource("System"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Filter.ByExcluding(Matching.FromSource("Masstransit"))
                .Filter.ByExcluding(Matching.FromSource("Hangfire"))
                .Filter.ByExcluding(Matching.FromSource("Default"))
                .Filter.ByExcluding(Matching.FromSource("Endpoint"))
                .Filter.ByExcluding(Matching.FromSource("HostAddress"))
                .Filter.ByExcluding("Scope[?] = 'HealthReportCollector is collecting health checks results.'") // Do not log health collector
                .Filter.ByExcluding(x => x.Properties.ContainsKey("AuthenticationScheme")) // Do not log 401s 

                // File
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Information'")
                    .WriteTo.File(new ExpressionTemplate(
                        "{ { ..@p, Timestamp: @t, Level: @l, Exception: @x, SourceContext: undefined(), ActionId: undefined() } }\r\n"),
                        loggingSettings.LoggingPath + appSettings.ApplicationId + "/INFO_.txt",
                        rollingInterval: RollingInterval.Day))

                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Error'")
                    .WriteTo.File(new ExpressionTemplate(
                            "{ { ..@p, Timestamp: @t, Level: @l, Exception: @x, SourceContext: undefined(), ActionId: undefined() } }\r\n"),
                            loggingSettings.LoggingPath + appSettings.ApplicationId + "/ERROR_.txt",
                            rollingInterval: RollingInterval.Day))

                 // Elk
                 .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Information'")
                    .WriteTo.File(new ElasticsearchJsonFormatter(), loggingSettings.LoggingPath + appSettings.ApplicationId + "/INFO_ELK_.txt")
                    .WriteTo.Elasticsearch(sinkOptions)
                    .WriteTo.OpenTelemetry(options =>
                     {
                         var header = new Dictionary<string, string>
                         {
                             { "Authorization", "Bearer o13QqDuUUk6nhXfYjy" }
                         };
                         options.Endpoint = "https://32b1b7da835640e1ad3de8874f2d0d51.apm.eu-west-1.aws.cloud.es.io:443";
                         options.Protocol = OtlpProtocol.Grpc;
                         options.Headers = header;
                     })
                    )

                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Error'")
                    .WriteTo.File(new ExceptionAsObjectJsonFormatter(), loggingSettings.LoggingPath + appSettings.ApplicationId + "/ERROR_ELK_.txt")
                    .WriteTo.Elasticsearch(sinkOptions)
                    .WriteTo.OpenTelemetry(options =>
                    {
                        var header = new Dictionary<string, string>
                         {
                             { "Authorization", "Bearer o13QqDuUUk6nhXfYjy" }
                         };
                        options.Endpoint = "https://32b1b7da835640e1ad3de8874f2d0d51.apm.eu-west-1.aws.cloud.es.io:443";
                        options.Protocol = OtlpProtocol.Grpc;
                        options.Headers = header;
                    })
                    )

                // Console
                .WriteTo.Console()

                // Create logger
                .CreateLogger();

            loggerFactory.AddSerilog();
        }
    }
}
