using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.ProductEndpoints;

internal static class DeleteProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete("/{id:long}", static async (long id, [FromServices] IProductService service) =>
        {
            await service.Delete(id).ConfigureAwait(false);
            return Results.NoContent();
        });
    }
}