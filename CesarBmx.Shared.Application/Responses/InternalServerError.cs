


using System;
using Newtonsoft.Json;

namespace CesarBmx.Shared.Application.Responses
{
    public class InternalServerError : Error
    {
        [JsonProperty(Order = 2)]
        public Guid ExceptionId { get; set; }

        public InternalServerError(string message)
            : base(500, message)
        {
            ExceptionId = Guid.NewGuid();
        }
    }
}