using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.ProductEndpoints;

internal static class DeleteProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:long}", static async (long id, [FromServices] ProductService productService) =>
        {
            await productService.Delete(id).ConfigureAwait(false);
            return Results.NoContent();
        });
    }
}