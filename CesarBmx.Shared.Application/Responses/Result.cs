using CesarBmx.Shared.Application.Types;
using System;

namespace CesarBmx.Shared.Application.Responses
{
    public class Result<TData, TValidationError>
        where TValidationError : struct, Enum
    {
        public TData Data { get; set; }
        public ResponseStatus Status { get; set; }
        public TValidationError? ValidationError { get; set; }

        public Result()
        {}
        public Result<TData, TValidationError> SetData(TData data)
        { 
            Data = data;    
            Status = ResponseStatus.SUCCESS;
            ValidationError = null;

            return this;
        }
        public Result<TData, TValidationError> SetValidationError (TValidationError error)
        {
            ValidationError = error;

            return this;
        }
    }
}