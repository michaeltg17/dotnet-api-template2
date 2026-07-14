using ApiClient.Extensions;
using AwesomeAssertions;
using Core.Testing.Builders;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Tests.Api.Endpoints.Products
{
    public abstract class ProductsTest : Test
    {
        public List<Product> initialProducts = new();

        public async ValueTask CreateProducts()
        {
            var request = new CreateProductRequestBuilder().Build();
            var tasks = new[]
            {
                ApiClient.CreateProduct(request).To<Product>(),
                ApiClient.CreateProduct(request).To<Product>(),
                ApiClient.CreateProduct(request).To<Product>()
            };
            initialProducts.AddRange(await Task.WhenAll(tasks));
        }

        public async Task ValidateInitialProductsAreTheSame(IEnumerable<long> exceptIds)
        {
            var dbProducts = await Context.Products.ToListAsync();
            var filteredProducts = dbProducts.Where(p => !exceptIds.Contains(p.Id)).ToList();
            var expectedProducts = initialProducts.Where(p => !exceptIds.Contains(p.Id)).ToList();
            filteredProducts.Should().BeEquivalentTo(expectedProducts);
        }
    }
}
