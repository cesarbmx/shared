

namespace CesarBmx.Shared.Application.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string message) : base(message)
        {}
    }
}
