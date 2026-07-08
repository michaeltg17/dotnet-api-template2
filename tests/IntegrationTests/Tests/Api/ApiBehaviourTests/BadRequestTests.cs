using AwesomeAssertions;
using System.Collections.Generic;
using Xunit;
using Core.Testing.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ApiClient.Extensions;
using Core.Testing.Extensions;
using Core.Testing;

namespace IntegrationTests.Tests.Api.ApiBehaviourTests
{
    [Collection(nameof(ApiCollection))]
    public class BadRequestTests : Test
    {
        public class BadRequestCase
        {
            public object Id;
            public object Date;
            public object? Request;
            public string ExpectedInstance;
            public string ExpectedDetail;
        }

        public static TheoryData<BadRequestCase> BadRequestCases()
        {
            return new TheoryData<BadRequestCase>
            {
                // Invalid: id cannot be parsed as long
                new BadRequestCase
                {
                    Id = "a", Date = null!, Request = null!,
                    ExpectedInstance = "/Test/Post/a",
                    ExpectedDetail = "Failed to bind parameter \"long id\" from \"a\"."
                },
                // Invalid: date cannot be parsed as DateTime
                new BadRequestCase
                {
                    Id = (long)1, Date = "b", Request = null!,
                    ExpectedInstance = "/Test/Post/1",
                    ExpectedDetail = "Failed to bind parameter \"DateTime date\" from \"b\"."
                },
                // Missing: request body not provided
                new BadRequestCase
                {
                    Id = (long)1, Date = "2020-01-01", Request = null!,
                    ExpectedInstance = "/Test/Post/1",
                    ExpectedDetail = "Required parameter \"TestPostRequest request\" was not provided from body."
                },
                // Invalid: JSON value cannot be converted to expected type
                new BadRequestCase
                {
                    Id = (long)1, Date = "2020-01-01", Request = "x",
                    ExpectedInstance = "/Test/Post/1",
                    ExpectedDetail = "Failed to read parameter \"TestPostRequest request\" from the request body as JSON. The JSON value could not be converted to Api.Models.Requests.TestPostRequest. Path: $ | LineNumber: 0 | BytePositionInLine: 3."
                },
                // Invalid: JSON property value cannot be converted to expected type
                new BadRequestCase
                {
                    Id = (long)1, Date = "2020-01-01",
                    Request = new Dictionary<string, object?> { ["id2"] = "notanumber" },
                    ExpectedInstance = "/Test/Post/1",
                    ExpectedDetail = "Failed to read parameter \"TestPostRequest request\" from the request body as JSON. The JSON value could not be converted to System.Int64. Path: $.id2 | LineNumber: 0 | BytePositionInLine: 19."
                }
            };
        }

        [Theory]
        [MemberData(nameof(BadRequestCases))]
        public async Task BadRequest_ExpectedProblemDetails(BadRequestCase testCase)
        {
            //When
            var response = await ApiClient.Test.Post(testCase.Id, testCase.Date, testCase.Request);

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            var traceId = problemDetails.GetTraceId();
            TraceIdValidator.IsValid(traceId).Should().BeTrue();

            var expected = new ProblemDetailsBuilder()
                .WithTraceId(traceId)
                .WithBadHttpRequestException()
                .WithInstance(testCase.ExpectedInstance)
                .WithDetail(testCase.ExpectedDetail)
                .Build();

            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}