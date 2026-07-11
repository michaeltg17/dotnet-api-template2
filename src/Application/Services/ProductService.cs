using Application.Models.Requests;
using System.Collections.Concurrent;
using Domain.Models;

namespace Application.Services
{
    public class ProductService
    {
        private readonly ConcurrentDictionary<long, Product> products = new();
        private long nextId = 1;

        public Task<Product?> GetById(long id)
        {
            products.TryGetValue(id, out var product);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            return Task.FromResult<IEnumerable<Product>>(products.Values);
        }

        public Task<Product> Create(CreateProductRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            var product = new Product
            {
                Id = Interlocked.Increment(ref nextId),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };
            products.TryAdd(product.Id, product);
            return Task.FromResult(product);
        }

        public Task<Product?> Update(long id, UpdateProductRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            if (!products.TryGetValue(id, out _))
                return Task.FromResult<Product?>(null);

            var product = new Product
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };
            products[id] = product;
            return Task.FromResult<Product?>(product);
        }

        public Task<bool> Delete(long id)
        {
            var removed = products.TryRemove(id, out _);
            return Task.FromResult(removed);
        }
    }
}