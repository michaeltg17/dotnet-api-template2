using Application.Models.Requests;
using System.Net.Http.Json;
using ApiClient.Endpoints;

namespace ApiClient
{
    public class ApiClient(HttpClient httpClient)
    {
        public TestEndpoints Test { get; } = new(httpClient);

        const string BasePath = "/api";

        public Task<HttpResponseMessage> GetAllProducts()
        {
            return httpClient.GetAsync($"{BasePath}/Products");
        }

        public Task<HttpResponseMessage> GetProduct(long id)
        {
            return GetProduct((object)id);
        }

        public Task<HttpResponseMessage> GetProduct(object id)
        {
            return httpClient.GetAsync($"{BasePath}/Products/{id}");
        }

        public Task<HttpResponseMessage> CreateProduct(CreateProductRequest request)
        {
            return httpClient.PostAsJsonAsync($"{BasePath}/Products", request);
        }

        public Task<HttpResponseMessage> UpdateProduct(long id, UpdateProductRequest request)
        {
            return UpdateProduct((object)id, request);
        }

        public Task<HttpResponseMessage> UpdateProduct(object id, UpdateProductRequest request)
        {
            return httpClient.PutAsJsonAsync($"{BasePath}/Products/{id}", request);
        }

        public async Task<HttpResponseMessage> DeleteProducts(DeleteProductsRequest request)
        {
            using var jsonRequest = new HttpRequestMessage(HttpMethod.Delete, $"{BasePath}/Products")
            {
                Content = JsonContent.Create(request),
            };
            return await httpClient.SendAsync(jsonRequest).ConfigureAwait(false);
        }
    }
}