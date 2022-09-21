using System;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class FluentValidationConfigConfig
    {
        public static IServiceCollection ConfigureFluentValidation(this IServiceCollection services, Type someValidator)
        {
            // Register all validators
            services.AddValidatorsFromAssemblyContaining(someValidator);

            // Automatic validation
            services.AddFluentValidationAutoValidation();

            // Apply rules to swagger
            services.AddFluentValidationRulesToSwagger();

            // Return
            return services;
        }
    }
}
