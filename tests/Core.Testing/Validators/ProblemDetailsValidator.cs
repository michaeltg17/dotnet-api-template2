using ApiClient.Extensions;
using Core.Testing.Builders;
using Core.Testing.Extensions;
using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Core.Testing.Validators
{
    public static class ProblemDetailsValidator
    {
        public static string ValidateTraceId(ProblemDetails problemDetails)
        {
            var traceId = problemDetails.GetTraceId();
            TraceIdValidator.IsValid(traceId).Should().BeTrue();
            return traceId;
        }

        public static async Task ValidateNotFoundException(HttpResponseMessage response, string entity, string route, long id)
        {
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = ValidateTraceId(problemDetails);

            var expected = new ProblemDetailsBuilder()
                .WithTraceId(traceId)
                .WithNotFoundException(entity, route, id)
                .Build();

            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
