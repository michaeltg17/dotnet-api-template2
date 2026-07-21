using Api.Endpoints.Test;
using Api.Endpoints;
using Api.Endpoints.Products;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var products = app.MapGroup("api/Products").DisableAntiforgery();
        GetAllProductsEndpoint.Map(products);
        GetProductEndpoint.Map(products);
        CreateProductEndpoint.Map(products);
        UpdateProductEndpoint.Map(products);
        DeleteProductsEndpoint.Map(products);

        var test = app.MapGroup("Test").DisableAntiforgery();
        GetOkEndpoint.Map(test);
        PostEndpoint.Map(test);
        ThrowInternalServerErrorEndpoint.Map(test);

        return app;
    }
}
