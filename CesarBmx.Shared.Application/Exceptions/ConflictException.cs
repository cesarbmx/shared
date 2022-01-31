

using System;

namespace CesarBmx.Shared.Application.Exceptions
{
    public class ConflictException: Exception
    {
        public object Conflict { get; set; }

        public ConflictException(object conflict)
        {
            Conflict = conflict;
        }
    }
}
