using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CesarBmx.Shared.Application.Responses;
using CesarBmx.Shared.Common.Extensions;

namespace CesarBmx.Shared.Api.ActionFilters
{
    public class ValidateRequestAttribute : IActionFilter
    {
        /// <summary>
        /// This filter distinguishes between 422 (validation) and 400 (malformed).
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                // 400 Bad Request
                foreach (var value in filterContext.ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        if (error.Exception != null || error.ErrorMessage.Contains("line ") && error.ErrorMessage.Contains("position "))
                        {
                            var errorResponse = new BadRequest(Application.Messages.ErrorMessage.BadRequest);
                            filterContext.Result = new ObjectResult(errorResponse) { StatusCode = 400 };
                            return;
                        }
                    }
                }

                // 422 Unprocessable Entity
                var errors = filterContext.ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

               
                var validationErrors = new List<ValidationError>();
                foreach (var error in errors)
                {
                    foreach (var value in error.Value)
                    {
                        // Handle DataAnotations Required
                        if (value.Contains("field is required"))
                        {
                            validationErrors.Add(new ValidationError( error.Key.ToFirstLetterLower(), Application.Messages.ErrorMessage.Required));
                        }
                        else // Handle fluent validations
                        {
                            validationErrors.Add(new ValidationError(error.Key.ToFirstLetterLower(), value));
                        }
                    }
                }
                var validationsResponse = new Validation(Application.Messages.ErrorMessage.ValidationFailed, validationErrors);

                filterContext.Result = new ObjectResult(validationsResponse) { StatusCode = 422 };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}