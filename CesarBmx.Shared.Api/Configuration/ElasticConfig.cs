using Microsoft.Extensions.DependencyInjection;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class ElasticConfig
    {
        public static IServiceCollection ConfigureSharedElastic(this IServiceCollection services)
        {

            services.AddAllElasticApm();

            return services;
        }
    }
}
