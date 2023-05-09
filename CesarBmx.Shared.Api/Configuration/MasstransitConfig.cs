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
using Humanizer.Configuration;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class MasstransitConfig
    {
        public static IServiceCollection ConfigureSharedMasstransit<TDbContext>(this IServiceCollection services, IConfiguration configuration, Type someConsumer, Type someSaga = null) 
            where TDbContext : DbContext
        {
            // Grab settings
            var appSettings = configuration.GetSection<AppSettings>();
            var rabbitMqSettings = configuration.GetSection<RabbitMqSettings>();

            services.AddMassTransit(x =>
            {
                // Publish
                //x.AddConsumersFromNamespaceContaining(someConsumer);
                x.AddConsumers(someConsumer.Assembly);

                if (someSaga != null)
                {
                    // Saga state machines
                    //x.AddSagaStateMachinesFromNamespaceContaining(someSaga);
                    x.AddSagaStateMachines(someSaga.Assembly);

                    // EF sagas
                    x.SetEntityFrameworkSagaRepositoryProvider(r =>
                    {
                        r.ExistingDbContext<TDbContext>();
                    });
                }

                // EF Outbox
                //x.AddEntityFrameworkOutbox<TDbContext>(o =>
                //{
                //    o.UseSqlServer();
                //    o.UseBusOutbox();
                //});


                //x.AddConfigureEndpointsCallback((provider, name, cfg) =>
                //{
                //    cfg.UseMessageRetry(r => r.Immediate(2));
                //    cfg.UseEntityFrameworkOutbox<TDbContext>(provider, x =>
                //    {

                //    });
                //});

                // Scheduler
                x.AddPublishMessageScheduler();

                // Request
                x.AddRequestClient<SubmitOrder>(new Uri($"exchange:OrderingApi:{nameof(SubmitOrder)}"));
                x.AddRequestClient<PlaceOrder>(new Uri($"exchange:OrderingApi:{nameof(PlaceOrder)}"));
                x.AddRequestClient<CancelOrder>(new Uri($"exchange:OrderingApi:{nameof(CancelOrder)}"));
                x.AddRequestClient<SendMessage>(new Uri($"exchange:NotificationApi:{nameof(SendMessage)}"));

                // RabbitMq
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });                   
                    cfg.MessageTopology.SetEntityNameFormatter(new SimpleNameFormatter(cfg.MessageTopology.EntityNameFormatter, appSettings));
                    cfg.ConfigureEndpoints(context);
                });
            });


            // Send
            EndpointConvention.Map<SubmitOrder>(new Uri($"exchange:OrderingApi:{nameof(SubmitOrder)}"));
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
