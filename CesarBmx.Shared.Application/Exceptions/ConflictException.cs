

using System;

namespace CesarBmx.Shared.Application.Exceptions
{
    public class ConflictException: Exception
    {
        public object Response { get; set; }

        public ConflictException(object response)
        {
            Response = response;
        }
    }
}
