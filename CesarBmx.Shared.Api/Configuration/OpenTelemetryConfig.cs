﻿using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using CesarBmx.Shared.Application.Settings;
using CesarBmx.Shared.Common.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using System;
using System.Reflection;
using OpenTelemetry.Metrics;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class OpenTelemetryConfig
    {
        public static IServiceCollection ConfigureSharedOpenTelemetry(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();
            var openTelemetrySettings = configuration.GetSection<OpenTelemetrySettings>();

            // Add OpenTelemetry
            services.AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddRuntimeInstrumentation()                
                .AddPrometheusExporter())
            .WithTracing(builder => builder
                .AddSource(appSettings.ApplicationId)
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName: appSettings.ApplicationId, serviceVersion: assembly.VersionNumber()))
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddJaegerExporter(
                 opts =>
                 {
                     opts.AgentHost = openTelemetrySettings.JaegerAgentHost;
                     opts.AgentPort = Convert.ToInt32(openTelemetrySettings.JaegerAgentPort);
                     opts.Protocol = JaegerExportProtocol.UdpCompactThrift;
                 }
                 ));


            return services;
        }

        public static IApplicationBuilder ConfigureSharedOpenTelemetry(this IApplicationBuilder app)
        {
            app.UseOpenTelemetryPrometheusScrapingEndpoint();

            return app;
        }
    }
}
