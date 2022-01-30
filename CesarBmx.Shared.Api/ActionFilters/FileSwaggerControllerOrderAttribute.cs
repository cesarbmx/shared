using System;

namespace CesarBmx.Shared.Api.ActionFilters
{
    /// <summary>
    /// Annotates a controller with a prefix sorting order that is used when generating the Swagger 
    /// documentation, in order to display the controllers in the desired order.
    /// </summary>
    public class SwaggerControllerOrderAttribute : Attribute
    {
        /// <summary>
        /// Determines the sorting order of the controller.
        /// </summary>
        public string OrderPrefix { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerControllerOrderAttribute"/> class.
        /// </summary>
        /// <param name="orderPrefix">Sets the sorting order prefix of the controller.</param>
        public SwaggerControllerOrderAttribute(string orderPrefix)
        {
            OrderPrefix = orderPrefix;
        }
    }
}