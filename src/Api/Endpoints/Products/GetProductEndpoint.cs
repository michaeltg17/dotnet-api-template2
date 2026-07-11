using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.ProductEndpoints;

internal static class GetProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/{id:long}", static async (long id, [FromServices] ProductService service) =>
        {
            var product = await service.GetById(id).ConfigureAwait(false);
            return product is { } ? Results.Ok(product) : Results.NotFound();
        });
    }
}