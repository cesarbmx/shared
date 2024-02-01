using System;


namespace CesarBmx.Shared.Application.Exceptions
{
    public class ConflictException: ApplicationException
    {
        public object Conflict { get; set; }

        public ConflictException(object conflict)
        {
            Conflict = conflict;
        }
    }
}
