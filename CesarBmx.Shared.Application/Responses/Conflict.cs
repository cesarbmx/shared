


namespace CesarBmx.Shared.Application.Responses
{
    public class Conflict<TReason> : Error
    {
        public TReason Reason { get; set; }

        public Conflict(TReason reason, string message)
        :base(409, message)
        {
            Reason = reason;
        }
    }
}