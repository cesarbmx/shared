using CesarBmx.Shared.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Net;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class HealthConfig
    {
        public static IServiceCollection ConfigureSharedHealth(this IServiceCollection services, IConfiguration configuration)
        {
            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();
            var rabbitMqSettings = configuration.GetSection<RabbitMqSettings>();

            // Health checks
            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint(appSettings.ApplicationId, $"http://{Dns.GetHostName()}/health");
                setup.SetEvaluationTimeInSeconds(60 * 10);
                setup.DisableDatabaseMigrations();
            }).AddInMemoryStorage();

            services.AddHealthChecks()
               .AddSqlServer(configuration.GetConnectionString(appSettings.DatabaseName), name: "SQL Server")
               .AddRabbitMQ(new Uri($"amqp://{rabbitMqSettings.Username}:{rabbitMqSettings.Password}@{rabbitMqSettings.Host}:5672"), name: "RabbitMQ")
               .AddSharedRedis(configuration);

            // Return
            return services;
        }
        public static IApplicationBuilder ConfigureSharedHealth(this IApplicationBuilder app)
        {
            // Api
            app.UseEndpoints(config =>
            {
                config.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            // UI
            app.UseHealthChecksUI(setup =>
            {
                setup.ApiPath = "/health-ui-api";
                setup.UIPath = "/health-ui";
            });


            // Return
            return app;
        }


        public static IHealthChecksBuilder AddSharedRabbitMQ(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            // Grab RabbitMQ settings
            var rabbitMqSettings = configuration.GetSection<RabbitMqSettings>();

            builder.AddRabbitMQ(name: "RabbitMQ", rabbitConnectionString: $"amqps://{rabbitMqSettings.Username}:{rabbitMqSettings.Password}@{rabbitMqSettings.Host}:{rabbitMqSettings.Port}/{rabbitMqSettings.VirtualHost}", failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded);
            return builder;
        }
        public static IHealthChecksBuilder AddSharedRedis(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var configurationOptions = configuration.GetConfigurationOptions();
            var conn = ConnectionMultiplexer.Connect(configurationOptions);
            builder.AddRedis(conn, name: "Redis");

            return builder;
        }
    }
}
