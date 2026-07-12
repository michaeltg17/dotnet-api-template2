using ApiClient.Extensions;
using AwesomeAssertions;
using Core.Testing.Builders;
using Domain.Models;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests.Api.Endpoints.Products
{
    [Collection(nameof(ApiCollection))]
    public class GetAllProductsEndpointTests : Test
    {
        [Fact]
        public async Task GetProductsOk()
        {
            //Given
            IEnumerable<Product> expectedProducts = [new ProductBuilder().Build(), new ProductBuilder().Build()];
            await Context.Products.AddRangeAsync(expectedProducts);
            await Context.SaveChangesAsync();

            //When
            var response = await ApiClient.GetAllProducts();

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var products = await response.To<List<Product>>();
            products.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task NoProducts_ReturnsOk()
        {
            //When
            var response = await ApiClient.GetAllProducts();

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var products = await response.To<List<Product>>();
            products.Should().BeEmpty();
        }
    }
}