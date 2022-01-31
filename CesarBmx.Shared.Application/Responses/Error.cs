


using Newtonsoft.Json;

namespace CesarBmx.Shared.Application.Responses
{
    public abstract class Error
    {
        [JsonProperty(Order = 1)]
        public int Code { get; set; }
        [JsonProperty(Order = 3)]
        public string Message { get; set; }

        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}