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
            services.AddConfiguration<AuthenticationSettings>(configuration);
            services.AddConfiguration<EnvironmentSettings>(configuration);

            return services;
        }
        public static void AddConfiguration<T>(
                this IServiceCollection services,
                IConfiguration configuration)
                where T : class
        {
            var instance = Activator.CreateInstance<T>();
            new ConfigureFromConfigurationOptions<T>(configuration.GetSection(nameof(T)))
                .Configure(instance);
            services.AddSingleton(instance);
        }
    }
}

