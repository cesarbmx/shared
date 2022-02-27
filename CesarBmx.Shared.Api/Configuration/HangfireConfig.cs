﻿using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CesarBmx.Shared.Api.ActionFilters;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class HangfireConfig
    {
        public static IServiceCollection ConfigureSharedHangfire(this IServiceCollection services)
        {
            // Return
            return services;
        }
        public static IApplicationBuilder ConfigureSharedHangfire(this IApplicationBuilder app, bool enableBasicAuthentication = true)
        {
            // Configure
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                // ReSharper disable once RedundantCast
                Authorization = new[] { enableBasicAuthentication ? (IDashboardAuthorizationFilter)new HangfireBasicAuthorizationFilter() : new HangfireNonAuthorizationFilter() },
                
            });
            app.UseHangfireServer();


            return app;
        }
    }
}
