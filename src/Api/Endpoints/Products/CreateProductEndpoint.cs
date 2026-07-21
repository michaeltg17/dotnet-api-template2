using Application.Services;
using Application.Models.Requests;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Products;

internal static class CreateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", static async (
            [FromForm] string name,
            [FromForm] string description,
            [FromForm] decimal price,
            [FromForm] IFormFile? image,
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

            var request = new CreateProductRequest
            {
                Name = name,
                Description = description,
                Price = price,
                ImageData = imageData,
                ImageFileName = imageFileName
            };
            var product = await productService.Create(request).ConfigureAwait(false);
            return Results.Created($"/api/Products/{product.Id}", product);
        });
    }
}