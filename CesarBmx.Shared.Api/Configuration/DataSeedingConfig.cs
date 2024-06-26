﻿using System;
using CesarBmx.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class DataSeedingConfig
    {
        public static IServiceCollection ConfigureSharedDataSeeding<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            var serviceProvider = services.BuildServiceProvider();
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                // Get MainDbContext
                var mainDbContext = serviceScope.ServiceProvider.GetService<TDbContext>();

                // Make sure it resolves
                if (mainDbContext == null) throw new ArgumentException("Not able to resolve MainDbContext");

                // Get EnvironmentSettings
                var environmentSettings = serviceScope.ServiceProvider.GetService<EnvironmentSettings>();

                // Make sure it resolves
                if (environmentSettings == null) throw new ArgumentException("Not able to resolve EnvironmentSettings");

                // Create database in development if it does not exist
                if (environmentSettings.Name == "Development")
                {
                    //mainDbContext.Database.Migrate();
                    mainDbContext.Database.EnsureCreated();
                }
            }

            // Return
            return services;
        }
    }
}
