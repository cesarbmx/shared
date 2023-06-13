

namespace CesarBmx.Shared.Application.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message) : base(message)
        {}
    }
}
