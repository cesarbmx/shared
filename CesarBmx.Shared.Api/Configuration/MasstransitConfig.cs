using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using CesarBmx.Shared.Application.Settings;
using CesarBmx.Shared.Api.Helpers;

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

           

            //services.AddMassTransit(x =>
            //{
            //    //x.AddEntityFrameworkOutbox<TDbContext>(o =>
            //    //{
            //    //    o.QueryDelay = TimeSpan.FromSeconds(1);

            //    //    o.UseSqlServer();
            //    //    o.UseBusOutbox();
            //    //});

            //    x.AddConsumer(someConsumer);

            //    x.UsingRabbitMq();

            //    //x.UsingRabbitMq((_, cfg) =>
            //    //{
            //    //    cfg.ConfigureEndpoints(_);
            //    //    cfg.AutoStart = true;
            //    //    cfg.Host(rabbitMqSettings.Host, "/", h =>
            //    //    {
            //    //        h.Username(rabbitMqSettings.Username);
            //    //        h.Password(rabbitMqSettings.Password);
            //    //    });
            //    //});

               
            //});

            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<TSomeComsumer>();
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

                //x.UsingInMemory((context, cfg) =>
                //{
                //    cfg.ConfigureEndpoints(context);
                //});
            });

            // Return
            return services;
        }
        public static IApplicationBuilder ConfigureSharedMasstransit(this IApplicationBuilder app)
        {           
            return app;
        }
    }
}
