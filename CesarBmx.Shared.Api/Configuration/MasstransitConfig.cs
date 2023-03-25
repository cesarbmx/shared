using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CesarBmx.Shared.Application.Settings;
using CesarBmx.Shared.Api.Helpers;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.Ordering.Commands;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class MasstransitConfig
    {
        public static IServiceCollection ConfigureSharedMasstransit<TDbContext, TSomeComsumer>(this IServiceCollection services, IConfiguration configuration) 
            where TDbContext : DbContext
            where TSomeComsumer : IConsumer
        {
            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();
            var rabbitMqSettings = configuration.GetSection<RabbitMqSettings>();

            services.AddMassTransit(x =>
            {
                // Publish
                x.AddConsumersFromNamespaceContaining<TSomeComsumer>();

                // Request
                x.AddRequestClient<CancelOrder>(new Uri($"exchange:OrderingApi:{nameof(CancelOrder)}"));

                // RabbitMq
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                    cfg.MessageTopology.SetEntityNameFormatter(new SimpleNameFormatter(cfg.MessageTopology.EntityNameFormatter, appSettings));

         
                });
            });


            // Send
            EndpointConvention.Map<PlaceOrder>(new Uri($"exchange:OrderingApi:{nameof(PlaceOrder)}"));
            EndpointConvention.Map<SendMessage>(new Uri($"exchange:NotificationApi:{nameof(SendMessage)}"));

            


            // Return
            return services;
        }
        public static IApplicationBuilder ConfigureSharedMasstransit(this IApplicationBuilder app)
        {           
            return app;
        }
    }
}
