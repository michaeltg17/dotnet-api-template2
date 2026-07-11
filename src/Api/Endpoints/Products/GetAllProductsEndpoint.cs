using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.ProductEndpoints;

internal static class GetAllProductsEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/", static async ([FromServices] ProductService service) =>
        {
            var products = await service.GetAll().ConfigureAwait(false);
            return Results.Ok(products);
        });
    }
}