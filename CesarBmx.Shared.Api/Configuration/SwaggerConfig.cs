using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CesarBmx.Shared.Api.ActionFilters;
using CesarBmx.Shared.Api.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using CesarBmx.Shared.Api.ResponseExamples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class SwaggerConfig
    {
        /// <summary>
        /// Common configuration for swagger
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="appName">The name of the application (e.g. Fraud API)</param>
        /// <param name="type">Swagger example type</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSharedSwagger(this IServiceCollection services, string appName, Type type)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", 
                    new OpenApiSecurityScheme
                {
                    Description = @"Please enter the bearer token (e.g 'Bearer eyJhbGciOiJIUzI1Ni....')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                var assemblyName = type.Assembly.GetName();

                if (assemblyName.Version is { })
                    c.SwaggerDoc("v1",
                        new OpenApiInfo
                        {
                            Title = appName,
                            Version = assemblyName.Version.ToString()
                        });
                c.ExampleFilters();
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
                //c.OperationFilter<CamelCaseParameter>();

                //Find all controllers from the calling assembly (Swagger custom ordering)
                var order = GetControllerOrderMap(type.Assembly, Assembly.GetExecutingAssembly());

                //Order based on the prefix, if in the dictionary, otherwise, use the controller name as is
                if (order.Count > 0)
                    c.OrderActionsBy(d => order.TryGetValue(d.ActionDescriptor.RouteValues["controller"], out var value) ? value : d.ActionDescriptor.RouteValues["controller"]);

                // XML documentation file
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = assemblyName.Name + ".xml";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                c.IncludeXmlComments(commentsFile);

                // XML documentation file (BASE)
                commentsFileName = typeof(VersionController).Assembly.GetName().Name + ".xml";
                commentsFile = Path.Combine(baseDirectory, commentsFileName);
                c.IncludeXmlComments(commentsFile);
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // Inheritance
                c.UseAllOfForInheritance();

                // Polymorphism
                c.UseOneOfForPolymorphism();
            });

            // Add swagger examples
            services.AddSwaggerExamplesFromAssemblyOf(typeof(BadRequestExample), type);

            // Enums as strings
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
        public static IApplicationBuilder ConfigureSharedSwagger(this IApplicationBuilder app, string appName, string route = "")
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = route + "/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{httpReq.PathBase}" } };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("http://localhost:65012/api/v1/swagger.json", "CryptoWatcher API");
                c.SwaggerEndpoint("http://localhost:65014/v1/swagger.json", "Ordering API");
                c.SwaggerEndpoint("http://localhost:65016/v1/swagger.json", "Notification API");
                c.RoutePrefix = route;
            });

            return app;
        }
        private static Dictionary<string, string> GetControllerOrderMap(Assembly assembly, Assembly sharedAssembly)
        {
            //Find all controllers in assembly
            var types = new List<Type>();
            types.AddRange(assembly.GetTypes());
            types.AddRange(sharedAssembly.GetTypes());
            var controllerTypes = types.Where(t => typeof(ControllerBase).IsAssignableFrom(t));

            //Build the dictionary
            var orderMap = new Dictionary<string, string>(
                controllerTypes.Where(c => c.GetCustomAttributes<SwaggerOrderAttribute>().Any())
                    .Select(c => new { Name = StripControllerName(c.Name), c.GetCustomAttribute<SwaggerOrderAttribute>()?.OrderPrefix })
                    .ToDictionary(a => a.Name, a => a.OrderPrefix), StringComparer.OrdinalIgnoreCase);

            //Remove the Controller suffix from the name, if it exists
            string StripControllerName(string name)
            {
                var suffix = "Controller";
                if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                    // Return name with suffix stripped
                    return name.Substring(0, name.Length - suffix.Length);
                // Suffix not found, return name as is
                return name;
            }

            return orderMap;
        }
    }
}
