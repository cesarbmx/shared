using System.Collections.Generic;


namespace CesarBmx.Shared.Application.Responses
{
    public class ValidationFailed : Error
    {
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationFailed(string message, List<ValidationError> validationErrors)
        : base( 422, message)
        {
            ValidationErrors = validationErrors;
        }
    }
}