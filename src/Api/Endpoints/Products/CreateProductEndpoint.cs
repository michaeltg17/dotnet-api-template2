using Application.Services;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Products;

internal static class CreateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", static async (
            [FromBody] CreateProductRequest createProductRequest, 
            [FromServices] ProductService productService) =>
        {
            var product = await productService.Create(createProductRequest).ConfigureAwait(false);
            return Results.Ok(product);
        });
    }
}