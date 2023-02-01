using CesarBmx.Shared.Application.FakeResponses;
using CesarBmx.Shared.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.Shared.Api.ResponseExamples
{
    public class ValidationFailedExample : IExamplesProvider<Validation>
    {
        public Validation GetExamples()
        {
            return FakeError.GetFake_Validation();
        }
    }
}