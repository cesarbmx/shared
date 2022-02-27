using CesarBmx.Shared.Application.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class DataSeedingConfig
    {
        public static IApplicationBuilder ConfigureSharedDataSeeding<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
        {

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Get MainDbContext
                var mainDbContext = serviceScope.ServiceProvider.GetService<TDbContext>();

                // Get EnvironmentSettings
                var environmentSettings = serviceScope.ServiceProvider.GetService<EnvironmentSettings>();

                // Create database in development if it does not exist
                if (environmentSettings.EnvironmentName == "Development")
                {
                    //mainDbContext.Database.Migrate();
                    mainDbContext.Database.EnsureCreated();
                }
            }

            return app;
        }
    }
}
