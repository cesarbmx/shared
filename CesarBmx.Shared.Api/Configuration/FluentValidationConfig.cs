using System.Reflection;
using System.Text.Json;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class FluentValidationConfigConfig
    {
        public static IMvcBuilder ConfigureFluentValidation(this IMvcBuilder mvcBuilder, Assembly assembly)
        {
            mvcBuilder.AddFluentValidation(fv => fv
                .RegisterValidatorsFromAssembly(assembly)
                .RunDefaultMvcValidationAfterFluentValidationExecutes = true).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });



            return mvcBuilder;
        }
    }
}
