namespace Application.Models.Requests;

public record CreateProductRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public decimal Price { get; init; }
    public byte[]? ImageData { get; init; }
    public string? ImageFileName { get; init; }
}