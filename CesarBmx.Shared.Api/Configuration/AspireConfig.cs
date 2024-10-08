using Microsoft.Extensions.Hosting;


namespace CesarBmx.Shared.Api.Configuration
{
    public static class AspireConfig
    {
        public static void ConfigureSharedAspire(this IHostApplicationBuilder builder)
        {
            builder.AddSqlServerClient("SqlServer");
        }
    }
}
