using ApiClient.Extensions;
using Application.Models.Requests;
using AwesomeAssertions;
using Core.Testing.Builders;
using Core.Testing.Validators;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests.Api.Endpoints.Products
{
    [Collection(nameof(ApiCollection))]
    public class CreateProductEndpointTests : ProductsTest
    {
        [Fact]
        public async Task CreateProductOk()
        {
            //Given
            await CreateProducts();

            //When
            var request = new CreateProductRequestBuilder().Build();
            var response = await ApiClient.CreateProduct(request);
            var product = await response.To<Product>();

            //Then: retuns expected product
            var expected = new ProductBuilder()
                .WithValues(p =>
                {
                    p.Id = product.Id;
                    p.Name = request.Name;
                    p.Description = request.Description;
                    p.Price = request.Price;
                })
                .Build();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            product.Id.Should().BeGreaterThan(0);
            product.Should().BeEquivalentTo(expected);

            //Then: expected product in db
            var dbProduct = await Context.Products.FindAsync(product.Id);
            dbProduct.Should().BeEquivalentTo(expected);
        }
    }
}