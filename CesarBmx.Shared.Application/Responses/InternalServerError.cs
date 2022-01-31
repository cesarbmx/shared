


namespace CesarBmx.Shared.Application.Responses
{
    public class InternalServerError : Error
    {
        public InternalServerError(string message)
            : base( 500, message)
        { }
    }
}