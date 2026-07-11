using Application.Models.Requests;
using Domain.Models;

namespace Application.Services
{
    public interface IProductService
    {
        Task<Product?> GetById(long id);
        Task<IEnumerable<Product>> GetAll();
        Task<Product> Create(CreateProductRequest request);
        Task<Product?> Update(long id, UpdateProductRequest request);
        Task<bool> Delete(long id);
    }
}