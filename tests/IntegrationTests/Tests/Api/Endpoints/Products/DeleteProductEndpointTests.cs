using ApiClient.Extensions;
using AwesomeAssertions;
using Core.Testing.Builders;
using Core.Testing.Validators;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests.Api.Endpoints.Products
{
    [Collection(nameof(ApiCollection))]
    public class DeleteProductEndpointTests : ProductsTest
    {
        [Fact]
        public async Task DeleteOk()
        {
            //When
            var response = await ApiClient.DeleteProduct(initialProducts[1].Id);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var dbProduct = await Context.Products.FindAsync(initialProducts[1].Id);
            dbProduct.Should().BeNull();
            await ValidateInitialProductsAreTheSame([initialProducts[1].Id]);
        }

        [Fact]
        public async Task NonExistentProduct_ExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.DeleteProduct(1);

            //Then
            await ProblemDetailsValidator.ValidateNotFoundException(response, "Product", "Products", 1);
        }
    }
}