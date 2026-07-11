using Application.Services;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.ProductEndpoints;

internal static class CreateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("/", static async ([FromBody] CreateProductRequest request, [FromServices] ProductService service) =>
        {
            var product = await service.Create(request).ConfigureAwait(false);
            return Results.Ok(product);
        });
    }
}