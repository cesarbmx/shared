using Newtonsoft.Json;

namespace CesarBmx.Shared.Application.Responses
{
    public class Conflict<TReason> : Error
    {
        [JsonProperty(Order = 2)]
        public TReason Reason { get; set; }

        public Conflict(TReason reason, string message)
        :base(409, message)
        {
            Reason = reason;
        }
    }
}