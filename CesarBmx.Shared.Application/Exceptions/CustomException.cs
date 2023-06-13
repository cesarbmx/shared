

namespace CesarBmx.Shared.Application.Exceptions
{
    public abstract class CustomException: System.Exception
    {
        public CustomException(string message) : base(message)
        {}
    }
}
