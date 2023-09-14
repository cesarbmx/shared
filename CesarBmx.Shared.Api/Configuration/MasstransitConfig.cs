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
        public static IServiceCollection ConfigureSharedMasstransit<TDbContext>(this IServiceCollection services, IConfiguration configuration, Type someConsumer, Type someSaga = null, bool withPrefix = false) 
            where TDbContext : DbContext
        {
            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();
            var rabbitMqSettings = configuration.GetSection<RabbitMqSettings>();
            var environmentSettings = configuration.GetSection<EnvironmentSettings>();

            // Prefix
            var prefix = string.Empty;
            if (withPrefix)
            {
                prefix = environmentSettings.ShortName + "_" + appSettings.Team + "_" ;
            }            

            services.AddMassTransit(x =>
            {
                // Publish
                x.AddConsumers(someConsumer.Assembly);

                if (someSaga != null)
                {
                    // Saga state machines
                    x.AddSagaStateMachinesFromNamespaceContaining(someSaga);
                    x.AddSagaStateMachines(someSaga.Assembly);

                    // EF sagas
                    x.SetEntityFrameworkSagaRepositoryProvider(r =>
                    {
                        r.ExistingDbContext<TDbContext>();
                    });
                }

                // EF Outbox
                x.AddEntityFrameworkOutbox<TDbContext>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });


                x.AddConfigureEndpointsCallback((provider, name, cfg) =>
                {
                    cfg.UseMessageRetry(r => r.Immediate(2));
                    cfg.UseEntityFrameworkOutbox<TDbContext>(provider, x =>
                    {

                    });
                });

                // Scheduler
                x.AddPublishMessageScheduler();

                // Request
                x.AddRequestClient<PlaceOrder>(new Uri($"exchange:{prefix}Ordering:{nameof(PlaceOrder)}"));
                x.AddRequestClient<CancelOrder>(new Uri($"exchange:{prefix}Ordering:{nameof(CancelOrder)}"));
                x.AddRequestClient<SendNotification>(new Uri($"exchange:{prefix}Notification:{nameof(SendNotification)}"));

                // RabbitMq
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });                   
                    cfg.MessageTopology.SetEntityNameFormatter(new SimpleEntityNameFormatter(cfg.MessageTopology.EntityNameFormatter, prefix));
                    cfg.ConfigureEndpoints(context);
                });

                // Endpoint name formatter
                x.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(prefix, false));
            });


            // Send
            EndpointConvention.Map<PlaceOrder>(new Uri($"exchange:{prefix}Ordering:{nameof(PlaceOrder)}"));
            EndpointConvention.Map<CancelOrder>(new Uri($"exchange:{prefix}Ordering:{nameof(CancelOrder)}"));
            EndpointConvention.Map<SendNotification>(new Uri($"exchange:{prefix}Notification:{nameof(SendNotification)}"));           


            // Return
            return services;
        }
        public static IApplicationBuilder ConfigureSharedMasstransit(this IApplicationBuilder app)
        {           
            return app;
        }
    }
}
