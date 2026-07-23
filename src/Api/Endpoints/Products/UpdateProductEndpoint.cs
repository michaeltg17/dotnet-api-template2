using Application.Services;
using Application.Models.Requests;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Products;

internal static class UpdateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:long}", static async (
            long id,
            [FromForm] UpdateProductRequest request,
            ProductService productService) =>
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

            var product = await productService.Update(id, request).ConfigureAwait(false);
            return Results.Ok(product);
        });
    }
}