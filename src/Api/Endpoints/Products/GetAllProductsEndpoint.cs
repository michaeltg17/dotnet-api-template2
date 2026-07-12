using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Products;

internal static class GetAllProductsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/", static async ([FromServices] ProductService productService) =>
        {
            var products = await productService.GetAll().ConfigureAwait(false);
            return Results.Ok(products);
        });
    }
}