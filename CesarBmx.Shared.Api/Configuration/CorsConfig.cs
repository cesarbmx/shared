using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CesarBmx.Shared.Settings;
using CesarBmx.Shared.Api.Configuration;

namespace CesarBmx.Shared.Configuration
{
    public static class CorsConfig
    {
        public static IServiceCollection ConfigureSharedCors(this IServiceCollection services, IConfiguration configuration)
        {
            // Grab settings
            var environmentSettings = configuration.GetSection<EnvironmentSettings>();

            services.AddCors(options =>
            {
                if (environmentSettings.Name == "Development")
                {
                    options.AddPolicy("AllowOrigins",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
                }
                else
                {
                    var corsSettings = configuration.GetSection<CorsSettings>();
                    var allowedOrigins = corsSettings.AllowedOrigins.Split(";");

                    options.AddPolicy("AllowOrigins",
                    builder =>
                    {
                        builder
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
                }
            });

            return services;
        }
        public static IApplicationBuilder ConfigureSharedCors(this IApplicationBuilder app)
        {
            app.UseCors("AllowOrigins");

            return app;
        }
    }
}
