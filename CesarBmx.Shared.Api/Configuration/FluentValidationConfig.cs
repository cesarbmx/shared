using System;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class FluentValidationConfigConfig
    {
        public static IServiceCollection ConfigureFluentValidation(this IServiceCollection services, Type someValidator)
        {
            services.AddValidatorsFromAssemblyContaining(someValidator);

            // Return
            return services;
        }
    }
}
