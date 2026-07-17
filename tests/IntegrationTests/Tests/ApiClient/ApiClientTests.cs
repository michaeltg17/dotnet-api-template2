using AwesomeAssertions;
using Xunit;
using ApiClient.Extensions;
using Domain.Models;
using ApiClient.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Core.Testing;
using Core.Testing.Extensions;
using IntegrationTests.Collections;
using Core.Testing.Extensions;

namespace IntegrationTests.Tests.ApiClient
{
    [Collection(nameof(DevelopmentApiCollection))]
    public class ApiClientTests : Test
    {
        [Fact]
        public async Task InternalServerError_ApiExceptionIsThrownWithExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.Test.ThrowInternalServerError();

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            TraceIdValidator.IsValid(problemDetails.TraceId!).Should().BeTrue();

            var expectedMessage = $$"""
                {
                  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                  "title": "InternalServerError",
                  "status": 500,
                  "detail": "Internal server error. Please contact the API support.",
                  "instance": "/Test/ThrowInternalServerError",
                  "traceId": "{{problemDetails.TraceId}}"
                }
                """;

            var func = response.To<Product>;
            await func.Should().ThrowAsync<ApiException>().WithMessage(expectedMessage);
        }

        [Fact]
        public async Task NoContent_ApiClientExceptionIsThrown()
        {
            //When
            var response = await ApiClient.Test.GetOk();

            //Then
            var func = response.To<ProblemDetails>;
            await func.Should().ThrowAsync<ApiClientException>().WithMessage("Response content is null, empty or whitespace.");
        }
    }
}
