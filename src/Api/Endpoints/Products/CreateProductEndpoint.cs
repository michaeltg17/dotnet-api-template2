using Application.Services;
using Application.Models.Requests;
using Application.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = Application.Models.File;

namespace Api.Endpoints.Products;

internal static class CreateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", static async (
            [FromForm] CreateProductRequest request,
            [FromServices] ProductService productService) =>
        {
            byte[]? imageData = null;
            string? imageFileName = null;
            if (image != null)
            {
                imageData = new byte[image.Length];
                using var stream = image.OpenReadStream();
                await stream.ReadExactlyAsync(imageData).ConfigureAwait(false);
                imageFileName = image.FileName;
            }

            var product = await productService.Create(request).ConfigureAwait(false);
            return Results.Created($"/api/Products/{product.Id}", product);
        });
    }
}