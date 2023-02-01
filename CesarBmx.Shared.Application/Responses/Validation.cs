using System.Collections.Generic;
using Newtonsoft.Json;


namespace CesarBmx.Shared.Application.Responses
{
    public class Validation : Error
    {
        [JsonProperty(Order = 2)]
        public List<ValidationError> ValidationErrors { get; set; }

        public Validation(string message, List<ValidationError> validationError)
        : base( 422, message)
        {
            ValidationErrors = validationError;
        }
    }
}