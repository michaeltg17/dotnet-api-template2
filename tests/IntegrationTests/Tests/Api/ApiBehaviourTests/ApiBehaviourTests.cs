using AwesomeAssertions;
using Xunit;
using Core.Testing.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ApiClient.Extensions;
using Core.Testing.Extensions;
using Core.Testing;
using Api.Models.Requests;

namespace IntegrationTests.Tests.Api.ApiBehaviourTests
{
    [Collection(nameof(ApiCollection))]
    public class ApiBehaviourTests : Test
    {
        [Fact]
        public async Task InternalServerError_ExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.Test.ThrowInternalServerError();

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = problemDetails.GetTraceId();
            TraceIdValidator.IsValid(traceId).Should().BeTrue();

            var expected = new ProblemDetailsBuilder()
                .WithTraceId(traceId)
                .WithInternalServerError("/Test/ThrowInternalServerError")
                .Build();

            var responseAsString = (await response.Content.ReadAsStringAsync()).ToLowerInvariant();
            responseAsString.Should().NotContain("Sensitive data".ToLowerInvariant());
            responseAsString.Should().NotContain("Exception".ToLowerInvariant());
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task NonexistentRoute_ExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.Test.RequestUnexistingRoute();

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = problemDetails.GetTraceId();
            TraceIdValidator.IsValid(traceId).Should().BeTrue();

            var expected = new ProblemDetailsBuilder()
                .WithTraceId(traceId)
                .WithNotFound()
                .Build();

            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ValidRequest_Ok()
        {
            //When
            var response = await ApiClient.Test.Post(1L, new DateTime(2020, 1, 1), new TestPostRequest { Id2 = 2L });

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}