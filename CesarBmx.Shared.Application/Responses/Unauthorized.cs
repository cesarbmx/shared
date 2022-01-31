


namespace CesarBmx.Shared.Application.Responses
{
    public class Unauthorized : Error
    {
        public Unauthorized(string message)
            : base( 401, message)
        { }
    }
}