﻿using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using CesarBmx.Shared.Application.Settings;
using CesarBmx.Shared.Common.Extensions;
using Serilog;
using Serilog.Filters;
using Serilog.Templates;
using System;
using System.Reflection;
using Serilog.Sinks.Elasticsearch;
using Serilog.Formatting.Elasticsearch;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class SerilogConfig
    {
        public static IHostBuilder ConfigureSharedSerilog(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog();

            return hostBuilder;
        }
        public static void ConfigureSharedSerilog(this IApplicationBuilder app, ILoggerFactory loggerFactory, Assembly assembly, IConfiguration configuration)
        {
            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();
            var environmentSettings = configuration.GetSection<EnvironmentSettings>();
            var loggingettings = configuration.GetSection<LoggingSettings>();
            var openTelemetrySettings = configuration.GetSection<OpenTelemetrySettings>();

            Log.Logger = new LoggerConfiguration()

                // Common fields
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Team", "CustomerTeam")
                .Enrich.WithProperty("App", appSettings.ApplicationId)
                .Enrich.WithProperty("Version", assembly.VersionNumber())
                .Enrich.WithProperty("Environment", environmentSettings.Name)

                // Filter
                .Filter.ByExcluding(Matching.FromSource("System"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Filter.ByExcluding(Matching.FromSource("Hangfire"))
                .Filter.ByExcluding(Matching.FromSource("Default"))
                .Filter.ByExcluding("Scope[?] = 'HealthReportCollector is collecting health checks results.'") // Do not log health collector
                .Filter.ByExcluding(x => x.Properties.ContainsKey("AuthenticationScheme")) // Do not log 401s 
                            

                // INFO
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Information'")
                    .WriteTo.File(new ExpressionTemplate(
                        "{ { ..@p, Timestamp: @t, Level: @l, Exception: @x, SourceContext: undefined(), ActionId: undefined() } }\r\n"),
                        loggingettings.LoggingPath + appSettings.ApplicationId + "\\INFO_.txt",
                        rollingInterval: RollingInterval.Day))

                // ERROR
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly("@l = 'Error'")
                    .WriteTo.File(new ExpressionTemplate(
                        "{ { ..@p, Timestamp: @t, Level: @l, Exception: @x, SourceContext: undefined(), ActionId: undefined() } }\r\n"),
                        loggingettings.LoggingPath + appSettings.ApplicationId + "\\ERROR_.txt",
                        rollingInterval: RollingInterval.Day))

                // Console
                .WriteTo.Console(new ExpressionTemplate(
                        "{ @x } { @p['ExecutionTime'] }\t{ @p['Event'] }" + Environment.NewLine))

                // Elasticsearch
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(openTelemetrySettings.ElasticsearchUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{appSettings.ApplicationId}-{environmentSettings.Name}-{DateTime.UtcNow:yyyy-MM}",
                    CustomFormatter = new ElasticsearchJsonFormatter()
                })

                // Create logger
                .CreateLogger();

            loggerFactory.AddSerilog();
        }
    }
}
