﻿using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using CesarBmx.Shared.Application.Settings;
using CesarBmx.Shared.Common.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Templates;
using System;
using System.Reflection;

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
            // Grab AppSettings
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);

            // Grab EnvironmentSettings
            var environmentSettings = new EnvironmentSettings();
            configuration.GetSection("EnvironmentSettings").Bind(environmentSettings);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("App", appSettings.ApplicationId)
                .Enrich.WithProperty("Version", assembly.VersionNumber())
                .Enrich.WithProperty("Environment", environmentSettings.EnvironmentName)
                .Enrich.WithProperty("Id", Guid.NewGuid())
                .WriteTo.File(new ExpressionTemplate(
                        "{ { ..@p, Timestamp: @t, Level: @l, Exception: @x, SourceContext: undefined(), ActionId: undefined(), ActionName: undefined() } }" + Environment.NewLine), "./Logs/log-.log",
                    rollingInterval: RollingInterval.Day,
                    //retainedFileCountLimit: 31,
                    //flushToDiskInterval: TimeSpan.FromSeconds(5),
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Console(new ExpressionTemplate(
                        "{ @p['Event'] }" + Environment.NewLine))
                .MinimumLevel.Override("Default", LogEventLevel.Information)
                .Filter.ByExcluding(Matching.FromSource("System"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Filter.ByExcluding(Matching.FromSource("Hangfire"))
                .Filter.ByExcluding(Matching.FromSource("Default"))
                .Filter.ByExcluding("Scope[?] = 'HealthReportCollector is collecting health checks results.'")
                .CreateLogger();

            loggerFactory.AddSerilog();
            //app.UseSer
        }
    }
}
