using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using CesarBmx.Shared.Application.Settings;
using CesarBmx.Shared.Api.Helpers;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.CryptoWatcher.Commands;
using CesarBmx.Shared.Messaging.CryptoWatcher.Events;

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
                x.AddRequestClient<CancelOrder>(new Uri($"exchange:CryptoWatcherApi:{nameof(CancelOrder)}"));

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
            EndpointConvention.Map<AddOrder>(new Uri($"exchange:CryptoWatcherApi:{nameof(AddOrder)}"));
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
