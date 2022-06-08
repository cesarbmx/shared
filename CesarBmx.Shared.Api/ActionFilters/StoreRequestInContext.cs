using Microsoft.AspNetCore.Mvc.Filters;

namespace CesarBmx.Shared.Api.ActionFilters
{
    public class StoreRequestInContextAttribute : IActionFilter
    {
        /// <summary>
        /// We store the request in the context so that we can grab it in case of an exception and properly log it.
        /// </summary>
        /// <param name="filterContext"></param>
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