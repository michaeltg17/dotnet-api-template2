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
        public async Task EmptyList_ReturnsOk()
        {
            //When
            var response = await ApiClient.GetAllProducts();

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var products = await response.To<List<Product>>();
            products.Should().BeEmpty();
        }

        [Fact]
        public async Task GetProductsOk()
        {
            //Given
            IEnumerable<Product> products = [new ProductBuilder().Build(), new ProductBuilder().Build()];
            Context.Products.AddRange(products);
            await Context.SaveChangesAsync();

            var response = await ApiClient.GetAllProducts();
            var products = await response.To<List<Product>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var actual = products[0];
            actual.Name.Should().Be(expected.Name);
            actual.Description.Should().Be(expected.Description);
            actual.Price.Should().Be(expected.Price);
        }
    }
}