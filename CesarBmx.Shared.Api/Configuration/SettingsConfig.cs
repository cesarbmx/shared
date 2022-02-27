using System;
using CesarBmx.Shared.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class SettingsConfig
    {
        public static IServiceCollection ConfigureSharedSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<AuthenticationSettings>(configuration, "AuthenticationSettings");
            services.AddConfiguration<EnvironmentSettings>(configuration, "EnvironmentSettings");

            return services;
        }
        public static void AddConfiguration<T>(
                this IServiceCollection services,
                IConfiguration configuration,
                string configurationTag = null)
                where T : class
        {
            if (string.IsNullOrEmpty(configurationTag))
            {
                configurationTag = typeof(T).Name;
            }

            var instance = Activator.CreateInstance<T>();
            new ConfigureFromConfigurationOptions<T>(configuration.GetSection(configurationTag))
                .Configure(instance);
            services.AddSingleton(instance);
        }
    }
}

