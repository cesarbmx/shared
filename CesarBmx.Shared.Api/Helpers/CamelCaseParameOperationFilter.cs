using System.Collections.Generic;
using CesarBmx.Shared.Common.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CesarBmx.Shared.Api.Helpers
{
    public class CamelCaseParameOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            else
            {
                foreach (var item in operation.Parameters)
                {
                    item.Name = item.Name.ToFirstLetterLower();
                }
            }
        }
    }
}