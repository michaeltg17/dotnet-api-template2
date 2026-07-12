using ApiClient.Extensions;
using Core.Testing.Builders;
using Core.Testing.Extensions;
using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Domain.Models;

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

        public static async Task ValidateNotFoundException<T>(HttpResponseMessage response, long id) where T : Entity
        {
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = ValidateTraceId(problemDetails);

            var expected = new ProblemDetailsBuilder()
                .WithTraceId(traceId)
                .WithNotFoundException<T>(id)
                .Build();

            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
