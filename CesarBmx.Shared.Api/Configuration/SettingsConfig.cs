using System;
using System.IO;
using System.Reflection;
using CesarBmx.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class SettingsConfig
    {
        public static IServiceCollection ConfigureSharedSettings(this IServiceCollection services, IConfigurationManager configuration)
        {
            AddJsonFiles(configuration);

            services.AddConfiguration<AppSettings>(configuration);
            services.AddConfiguration<AuthenticationSettings>(configuration);
            services.AddConfiguration<EnvironmentSettings>(configuration);
            services.AddConfiguration<CorsSettings>(configuration);
            services.AddConfiguration<RabbitMqSettings>(configuration);

            return services;
        }
        public static void AddConfiguration<T>(
                this IServiceCollection services,
                IConfiguration configuration)
                where T : class
        {
            var key = typeof(T).Name;
            var instance = Activator.CreateInstance<T>();
            new ConfigureFromConfigurationOptions<T>(configuration.GetSection(key))
                .Configure(instance);
            services.AddSingleton(instance);
        }
        public static T GetSection<T>(this IConfiguration configuration)
               where T : class, new()
        {
            var key = typeof(T).Name;
            var settings = new T();    
            configuration.GetSection(key).Bind(settings);

            return settings;
        }
        private static void AddJsonFiles(IConfigurationManager configurationManager)
        {
            var binDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            configurationManager
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings_shared.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}

