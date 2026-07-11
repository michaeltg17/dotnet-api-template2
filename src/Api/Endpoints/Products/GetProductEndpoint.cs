using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.ProductEndpoints;

internal static class GetProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:long}", static async (long id, [FromServices] ProductService productService) =>
        {
            var product = await productService.GetById(id).ConfigureAwait(false);
            return product is { } ? Results.Ok(product) : Results.NotFound();
        });
    }
}