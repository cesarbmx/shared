using Microsoft.AspNetCore.Mvc.Filters;

namespace CesarBmx.Shared.Api.ActionFilters
{
    public class StoreRequestInContextAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Method == "POST")
            {
                var request = filterContext.ActionArguments["request"];
                filterContext.HttpContext.Items["Request"] = request;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}