using System;
using CesarBmx.Shared.Api.ActionFilters;
using CesarBmx.Shared.Api.Controllers;
using CesarBmx.Shared.Application.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using FluentValidation.AspNetCore;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class MvcConfig
    {
        public static IServiceCollection ConfigureSharedMvc(this IServiceCollection services, IConfiguration configuration, bool enableRazorPages)
        {
            // Grab AuthenticationSettings
            var authenticationSettings = new AuthenticationSettings();
            configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddControllers(
                    config =>
                    {
                        // Authentication
                        if (authenticationSettings.Enabled)
                        {
                            // Authentication
                            var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                            config.Filters.Add(new AuthorizeFilter(policy));
                        }

                        // Filters
                        config.Filters.Add(typeof(ValidateRequestAttribute));
                        config.Filters.Add(typeof(StoreRequestInContextAttribute));
                        config.Filters.Add(typeof(Identity));
                    })
                .ConfigureSharedSerialization();

            if(enableRazorPages) services.AddRazorPages();

            services.AddRouting(options => options.LowercaseUrls = true);

            // Allow synchronous IO (elmah css was not loading)
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });

            services.AddControllers()
                .AddApplicationPart(typeof(VersionController).Assembly);

            return services;
        }

        public static IApplicationBuilder ConfigureSharedMvc(this IApplicationBuilder app, IConfiguration configuration, bool enableRazorPages)
        {
            // Grab AuthenticationSettings
            var authenticationSettings = new AuthenticationSettings();
            configuration.GetSection("Authentication").Bind(authenticationSettings);

            app.UseRouting();
            if (authenticationSettings.Enabled)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
            app.UseEndpoints(endpoints =>
            {
                if (enableRazorPages) endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
            app.UseStaticFiles();

            return app;
        }
    }
}
