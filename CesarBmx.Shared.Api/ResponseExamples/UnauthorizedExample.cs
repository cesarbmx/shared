﻿using CesarBmx.Shared.Application.FakeResponses;
using CesarBmx.Shared.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.Shared.Api.ResponseExamples
{
    public class UnauthorizedExample : IExamplesProvider<Unauthorized>
    {
        public Unauthorized GetExamples()
        {
            return FakeError.GetFake_Unauthorized();
        }
    }
}