﻿using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using CesarBmx.Shared.Api.Helpers;
using CesarBmx.Shared.Application.Responses;
using CesarBmx.Shared.Application.Settings;
using Microsoft.AspNetCore.Server.IISIntegration;
using ErrorMessage = CesarBmx.Shared.Application.Messages.ErrorMessage;

namespace CesarBmx.Shared.Api.Configuration
{
    /// <summary>
    /// It enables authentication ()
    /// </summary>
    public static class AuthenticationConfig
    {
        public static IServiceCollection UseSharedAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Grab AuthenticationSettings
            var authenticationSettings = new AuthenticationSettings();
            configuration.GetSection("Authentication").Bind(authenticationSettings);

            if (authenticationSettings.Enabled)
            {
                switch (authenticationSettings.AuthenticationType)
                {
                    case "FAKE":
                        return services.UseSharedFakeAuthentication();
                    case "JWT":
                        return services.UseSharedJwtAuthentication(authenticationSettings);
                    case "API_KEY":
                        return services.UseSharedApiKeyAuthentication();
                    case "WINDOWS":
                        return services.UseSharedWindowsAuthentication();
                    default:
                        throw new ApplicationException("AuthenticationType not supported: " + authenticationSettings.AuthenticationType);
                }
            }

            return services;
        }
        private static IServiceCollection UseSharedJwtAuthentication(this IServiceCollection services, AuthenticationSettings authenticationSettings)
        {
            // Configure JWT authentication
            var key = Encoding.ASCII.GetBytes(authenticationSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = authenticationSettings.Issuer,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    x.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            // First, get the value from the cookie. If no cookie present, then use the authorization header
                            context.Token =
                                context.Request.Cookies["AccessToken"] ??
                                context.Request.Headers["Authorization"];
                            return Task.CompletedTask;
                        },
                        // Add body to 401/403
                        OnChallenge = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json; charset=utf-8";
                            var errorResponse = new Unauthorized(ErrorMessage.Unauthorized);
                            var result = JsonConvert.SerializeObject(errorResponse);
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json; charset=utf-8";
                            var errorResponse = new Unauthorized(ErrorMessage.Forbidden);
                            var result = JsonConvert.SerializeObject(errorResponse);
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            return services;
        }
        private static IServiceCollection UseSharedFakeAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("FakeAuthentication")
                .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("FakeAuthentication", null);

            return services;
        }
        private static IServiceCollection UseSharedApiKeyAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("ApiKeyAuthentication")
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKeyAuthentication", null);

            return services;
        }
        private static IServiceCollection UseSharedWindowsAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            return services;
        }
    }
}