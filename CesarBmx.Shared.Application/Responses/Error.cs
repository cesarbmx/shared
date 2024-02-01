using Newtonsoft.Json;


namespace CesarBmx.Shared.Application.Responses
{
    public abstract class Error
    {
        [JsonProperty(Order = 1)]
        public int StatusCode { get; set; }
        [JsonProperty(Order = 3)]
        public string Message { get; set; }

        public Error(int statuCode, string message)
        {
            StatusCode = statuCode;
            Message = message;
        }
    }
}