


namespace CesarBmx.Shared.Application.Responses
{

    public class NotFound : Error
    {
        public NotFound(string message)
            : base( 404, message)
        { }
    }
}