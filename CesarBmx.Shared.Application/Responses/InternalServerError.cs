
using System;
using Newtonsoft.Json;

namespace CesarBmx.Shared.Application.Responses
{
    public class InternalServerError : Error
    {
        [JsonProperty(Order = 2)]
        public Guid Id { get; set; }

        public InternalServerError(string message, Guid id)
            : base(500, message)
        {
            Id = id;
        }
    }
}