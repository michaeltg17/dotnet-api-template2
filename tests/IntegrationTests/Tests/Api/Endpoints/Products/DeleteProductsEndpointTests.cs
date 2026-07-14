using ApiClient.Extensions;
using AwesomeAssertions;
using Core.Testing.Validators;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Xunit;

namespace IntegrationTests.Tests.Api.Endpoints.Products
{
    [Collection(nameof(ApiCollection))]
    public class DeleteProductsEndpointTests : ProductsTest
    {
        [Fact]
        public async Task DeleteSingleOk()
        {
            //When
            var response = await ApiClient.DeleteProducts([initialProducts[1].Id]);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body)!;
            result["deletedIds"].EnumerateArray().First().GetInt64().Should().Be(initialProducts[1].Id);
            result["notFoundIds"].EnumerateArray().Count().Should().Be(0);

            var dbProduct = await Context.Products.FindAsync(initialProducts[1].Id);
            dbProduct.Should().BeNull();
            await ValidateInitialProductsAreTheSame([initialProducts[1].Id]);
        }

        [Fact]
        public async Task DeleteMultipleOk()
        {
            //When
            var ids = new[] { initialProducts[0].Id, initialProducts[1].Id };
            var response = await ApiClient.DeleteProducts(ids);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body)!;
            result["deletedIds"].EnumerateArray().Select(e => e.GetInt64()).Should().BeEquivalentTo(ids);
            result["notFoundIds"].EnumerateArray().Count().Should().Be(0);

            var dbProducts = await Context.Products.ToListAsync();
            dbProducts.Should().HaveCount(1);
            dbProducts[0].Id.Should().Be(initialProducts[2].Id);
        }

        [Fact]
        public async Task NonExistentProduct_ExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.DeleteProducts([long.MaxValue]);

            //Then
            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Status.Should().Be((int)HttpStatusCode.NotFound);
            problemDetails.Title.Should().Be("NotFoundManyException");
            var notFoundIds = problemDetails.Extensions["NotFoundIds"] as List<object?>;
            notFoundIds.Should().NotBeNull();
            notFoundIds!.Count.Should().Be(1);
            notFoundIds[0].Should().Be(long.MaxValue);
        }

        [Fact]
        public async Task PartialNotFound_DeleteOnlyFound_OnlyExistingDeleted()
        {
            //When
            var ids = new[] { initialProducts[0].Id, long.MaxValue };
            var response = await ApiClient.DeleteProducts(ids, true);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body)!;
            result["deletedIds"].EnumerateArray().First().GetInt64().Should().Be(initialProducts[0].Id);
            result["notFoundIds"].EnumerateArray().First().GetInt64().Should().Be(long.MaxValue);
        }

        [Fact]
        public async Task AllNotFound_DeleteOnlyFound_ReturnsEmpty()
        {
            //When
            var ids = new[] { long.MaxValue - 1, long.MaxValue };
            var response = await ApiClient.DeleteProducts(ids, true);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body)!;
            result["deletedIds"].EnumerateArray().Count().Should().Be(0);
            result["notFoundIds"].EnumerateArray().Select(e => e.GetInt64()).Should().BeEquivalentTo(ids);
        }
    }
}