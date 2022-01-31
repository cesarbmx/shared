


namespace CesarBmx.Shared.Application.Responses
{
    public class BadRequest : Error
    {
        public BadRequest(string message)
        : base(400, message)
        { }
    }
}