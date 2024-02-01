using System;


namespace CesarBmx.Shared.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message)
        {}
    }
}
