using ApiClient.Extensions;
using AwesomeAssertions;
using Core.Testing.Builders;
using Core.Testing.Validators;
using IntegrationTests.Collections;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests.Api.ApiBehaviourTests
{
    [Collection(nameof(DevelopmentApiCollection))]
    public class DevelopmentApiBehaviourTests : Test
    {
        [Fact]
        public async Task InternalServerError_ExposesSensitiveData()
        {
            //When
            var response = await ApiClient.Test.ThrowInternalServerError();

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = ProblemDetailsValidator.ValidateTraceId(problemDetails);
            var exceptionText = ProblemDetailsValidator.ValidateException(problemDetails);

            var expected = new ProblemDetailsBuilder()
                .WithInternalServerError("Exception", "Sensitive data", "/Test/ThrowInternalServerError")
                .WithTraceId(traceId)
                .WithException(exceptionText)
                .Build();

            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}