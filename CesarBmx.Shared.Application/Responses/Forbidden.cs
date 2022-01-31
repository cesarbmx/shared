


namespace CesarBmx.Shared.Application.Responses
{
    public class Forbidden : Error
    {
        public Forbidden(string message)
            : base(403, message)
        { }
    }
}