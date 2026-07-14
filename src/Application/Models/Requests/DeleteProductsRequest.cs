namespace Application.Models.Requests;

public sealed record DeleteProductsRequest(
    long[] Ids,
    bool IgnoreNotFound = false
);