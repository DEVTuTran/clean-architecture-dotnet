using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> AddAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<bool> ExistsAsync(Guid id);
}