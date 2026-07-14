namespace Application.Models.Responses;

public sealed record DeleteProductsResponse(long[] DeletedIds, long[] NotFoundIds);