using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using CesarBmx.Shared.Settings;
using CesarBmx.Shared.Common.Extensions;
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
            var jaegerSettings = configuration.GetSection<JaegerSettings>();
            var elkSettings = configuration.GetSection<ElkSettings>();

            // Add OpenTelemetry
            services.AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter())
                //.AddOtlpExporter(
                // opts =>
                // {
                //     opts.Endpoint = jaegerSettings.JaegerAgentHost;
                //     opts.AgentPort = Convert.ToInt32(jaegerSettings.JaegerAgentPort);
                //     opts.Protocol = JaegerExportProtocol.UdpCompactThrift;
                // }))
            .WithTracing(builder => builder
                .AddSource(appSettings.ApplicationId)
                .AddSource("MassTransit")
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName: appSettings.ApplicationId, serviceVersion: assembly.VersionNumber()))
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddRedisInstrumentation()
                .AddJaegerExporter(
                 opts =>
                 {
                     opts.AgentHost = jaegerSettings.JaegerAgentHost;
                     opts.AgentPort = Convert.ToInt32(jaegerSettings.JaegerAgentPort);
                     opts.Protocol = JaegerExportProtocol.UdpCompactThrift;
                 }
                 )
                .AddOtlpExporter(opts =>
                {
                    opts.Protocol = OtlpExportProtocol.Grpc;
                    opts.Endpoint = new Uri(elkSettings.ElasticsearchUrl);
                    opts.Headers = "Authorization=Bearer " + elkSettings.ElasticsearchUrl;
                })
                );

            return services;
        }

        public static IApplicationBuilder ConfigureSharedOpenTelemetry(this IApplicationBuilder app)
        {
            app.UseOpenTelemetryPrometheusScrapingEndpoint();

            return app;
        }
    }
}
