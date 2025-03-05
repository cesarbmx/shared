using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class ElasticConfig
    {
        public static IServiceCollection ConfigureSharedElastic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAllElasticApm();

            return services;
        }
    }
}
