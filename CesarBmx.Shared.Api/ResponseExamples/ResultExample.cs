using CesarBmx.Shared.Application.Responses;
using CesarBmx.Shared.Application.Types;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace CesarBmx.Shared.Api.ResponseExamples
{
    public class ResultExample<TData, TValidationError, TDataExample> : IExamplesProvider<Result<TData, TValidationError>>
                    where TValidationError : struct, Enum
                    where TDataExample : IExamplesProvider<TData>, new()
    { 
        public Result<TData, TValidationError> GetExamples()
        {
            return new Result<TData, TValidationError>
            {
                Data =  new TDataExample().GetExamples(),
                Status = ResponseStatus.SUCCESS
            };
        }
    }
}