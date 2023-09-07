using System.Collections.Generic;
using Newtonsoft.Json;


namespace CesarBmx.Shared.Application.Responses
{
    public class ValidationFailed : Error
    {
        [JsonProperty(Order = 2)]
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationFailed(string message, List<ValidationError> validationError)
        : base( 422, message)
        {
            ValidationErrors = validationError;
        }
    }
}