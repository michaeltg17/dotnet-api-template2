using Application.Services;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.ProductEndpoints;

internal static class UpdateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPut("/{id:long}", static async (
            long id,
            [FromBody] UpdateProductRequest request,
            [FromServices] IProductService service) =>
        {
            var product = await service.Update(id, request).ConfigureAwait(false);
            return product is { } ? Results.Ok(product) : Results.NotFound();
        });
    }
}