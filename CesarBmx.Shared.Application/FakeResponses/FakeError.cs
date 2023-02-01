using System;
using System.Collections.Generic;
using CesarBmx.Shared.Application.Responses;


namespace CesarBmx.Shared.Application.FakeResponses
{
    public static class FakeError
    {
        public static BadRequest GetFake_BadRequest()
        {
            return new BadRequest( Messages.ErrorMessage.BadRequest);
        }
        public static NotFound GetFake_NotFound()
        {
            return new NotFound(Messages.ErrorMessage.NotFound);
        }
        public static Validation GetFake_Validation()
        {
            var validationErrorResponseList = new List<ValidationError>
            {
                new ValidationError("fieldName", "Validation description")
            };
            var validationResponse = new Validation(Messages.ErrorMessage.ValidationFailed, validationErrorResponseList);

            return validationResponse;
        }
        public static InternalServerError GetFake_InternalServerError()
        {
            return new InternalServerError(Messages.ErrorMessage.InternalServerError, Guid.NewGuid());
        }
        public static Unauthorized GetFake_Unauthorized()
        {
            return new Unauthorized( Messages.ErrorMessage.Unauthorized);
        }
        public static Forbidden GetFake_Forbidden()
        {
            return new Forbidden(Messages.ErrorMessage.Forbidden);
        }
    }
}
