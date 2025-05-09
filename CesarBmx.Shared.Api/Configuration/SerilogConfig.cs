using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CesarBmx.Shared.Settings;
using CesarBmx.Shared.Common.Extensions;
using Serilog;
using Serilog.Templates;
using System.Reflection;
using Serilog.Formatting.Elasticsearch;
using System;
using Serilog.Filters;
using Elastic.Serilog.Sinks;
using Elastic.Ingest.Elasticsearch;
using Elastic.Transport;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch.DataStreams;

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

            var expressionTemplate = new ExpressionTemplate("{ { ..@p, Timestamp: @t, Level: @l, Exception: @x, SourceContext: @c, ActionId: undefined() } }\r\n");

            //var sinkOptions = new ElasticsearchSinkOptions(new Uri(elkSettings.ElasticsearchUrl))
            //{
            //    AutoRegisterTemplate = true,
            //    ModifyConnectionSettings = x => x.GlobalHeaders(new NameValueCollection { { "Authorization", $"Bearer {elkSettings.BearerToken}" } }),
            //    IndexFormat = $"{environmentSettings.Name}-{appSettings.ApplicationId}-{DateTime.Now:yyyy-MM}",
            //    CustomFormatter = expressionTemplate
            //};


            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()

                // Common properties
                .Enrich.WithProperty("Team", "CustomerTeam")
                .Enrich.WithProperty("App", appSettings.ApplicationId)
                .Enrich.WithProperty("Version", assembly.VersionNumber())
                .Enrich.WithProperty("Environment", environmentSettings.Name)

                // Exclude everything else 
                //.Filter.ByExcluding(Matching.FromSource("System"))
                //.Filter.ByExcluding(Matching.FromSource("Microsoft"))
                //.Filter.ByExcluding(Matching.FromSource("MassTransit"))
                //.Filter.ByExcluding(Matching.FromSource("Hangfire"))
                //.Filter.ByExcluding(Matching.FromSource("Default"))
                .Filter.ByExcluding("Scope[?] = 'HealthReportCollector is collecting health checks results.'") // Do not log health collector
                                                                                                               //.Filter.ByExcluding(x => x.Properties.ContainsKey("AuthenticationScheme")) // Do not log 401s 

                // File INFO
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Information'")
                    .Filter.ByIncludingOnly(Matching.FromSource("CesarBmx"))
                    .WriteTo.File(expressionTemplate, loggingSettings.LoggingPath + appSettings.ApplicationId + "/INFO_.txt", rollingInterval: RollingInterval.Day)
                )

                // File ERROR
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Error'")
                    .WriteTo.File(expressionTemplate, loggingSettings.LoggingPath + appSettings.ApplicationId + "/ERROR_.txt", rollingInterval: RollingInterval.Day)
                )

                // Elk INFO
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Information'")
                    .Filter.ByIncludingOnly(Matching.FromSource("CesarBmx"))
                    .WriteTo.File(new ElasticsearchJsonFormatter(), loggingSettings.LoggingPath + appSettings.ApplicationId + "/INFO_ELK_.txt")
                     .WriteTo.Elasticsearch([new Uri(elkSettings.Endpoint)], options =>
                     {
                         options.DataStream = new DataStreamName($"INFO-{environmentSettings.Prefix}-{appSettings.ApplicationId}-{DateTime.UtcNow:yyyy-MM}");
                         options.TextFormatting = new EcsTextFormatterConfiguration();
                         options.BootstrapMethod = BootstrapMethod.Failure;
                     }, transport =>
                     {
                         transport.Authentication(new ApiKey(elkSettings.ApiKey));
                         transport.ServerCertificateValidationCallback((_, _, _, _) => true);
                     })
                )

                // Elk ERROR
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Error'")
                    .WriteTo.File(new ExceptionAsObjectJsonFormatter(), loggingSettings.LoggingPath + appSettings.ApplicationId + "/ERROR_ELK_.txt")
                    .WriteTo.Elasticsearch([new Uri(elkSettings.Endpoint)], options =>
                    {
                        options.DataStream = new DataStreamName($"ERROR-{environmentSettings.Prefix}-{appSettings.ApplicationId}-{DateTime.UtcNow:yyyy-MM}");
                        options.TextFormatting = new EcsTextFormatterConfiguration();
                        options.BootstrapMethod = BootstrapMethod.Failure;
                    }, transport =>
                    {
                        transport.Authentication(new ApiKey(elkSettings.ApiKey));
                        transport.ServerCertificateValidationCallback((_, _, _, _) => true);
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
