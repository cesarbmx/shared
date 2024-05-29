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
            var corsSettings = configuration.GetSection<CorsSettings>();

            // Allowed origins
            var allowedOrigins = corsSettings.AllowedOrigins.Split(";");

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigins",
                    builder =>
                    {
                            builder
							.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
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
