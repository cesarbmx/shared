using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using CesarBmx.Shared.Settings;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Hybrid;
using System;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class RedisConfig
    {
        public static void ConfigureSharedRedis(this IServiceCollection services, IConfiguration configuration)
        {
            // Grab App settings
            var appSettings = configuration.GetSection<AppSettings>();

            // Grab Environment settings
            var environmentSettings = configuration.GetSection<EnvironmentSettings>();

            var instanceName = $"{environmentSettings.Prefix.ToUpper()}_{appSettings.ApplicationId}:";

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = configuration.GetConfigurationOptions();
                options.InstanceName = instanceName;
            });

#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            services.AddHybridCache(options =>
            {
                options.MaximumPayloadBytes = 1024 * 1024;
                options.MaximumKeyLength = 1024;
                options.DefaultEntryOptions = new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromSeconds(10),                   
                    LocalCacheExpiration = TimeSpan.FromSeconds(10)
                };
            });
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }
        public static void AddSharedCache(this IServiceCollection services, IConfiguration configuration, string applicationId)
        {
            // Grab Environment settings
            var environmentSettings = configuration.GetSection<EnvironmentSettings>();

            var instanceName = $"{environmentSettings.Prefix.ToUpper()}_{applicationId}:";
            var options = new RedisCacheOptions
            {
                ConfigurationOptions = configuration.GetConfigurationOptions(),
                InstanceName = instanceName
            };

            services.AddKeyedSingleton<IDistributedCache>(applicationId,  new RedisCache(options));
        }
        public static ConfigurationOptions GetConfigurationOptions(this IConfiguration configuration)
        {
            // Grab Redis settings
            var redisSettings = configuration.GetSection<RedisSettings>();

            var configurationOptions = new ConfigurationOptions
            {
                //Ssl = true,
                //SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
                EndPoints = { $"{redisSettings.Url}:{redisSettings.Port}" },
                AbortOnConnectFail = false,               
                //AsyncTimeout = 2000,
                //SyncTimeout = 2000

            };
            configurationOptions.CertificateValidation += (sender, certificate, chain, errors) => true;           
            return configurationOptions;
        }
    }
}
