using Microsoft.AspNetCore.Builder;
using CesarBmx.Shared.Api.Middlewares;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class MiddlewareConfig
    {
        public static IApplicationBuilder ConfigureSharedMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            return app;
        }
    }
}
