namespace Application.Models.Requests;

public sealed record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price
) : CreateProductRequest(Name, Description, Price);