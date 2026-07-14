using Application.Models.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Products;

internal static class DeleteProductsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/", static async (
            [FromBody] DeleteProductsRequest request,
            [FromServices] ProductService productService) =>
        {
            var response = await productService.Delete(request).ConfigureAwait(false);
            return Results.Ok(response);
        });
    }
}