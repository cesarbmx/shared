using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace CesarBmx.Shared.Api.ActionFilters
{
    public class HangfireNonAuthorizationFilter :  IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {

            return true;
        }
    }
}