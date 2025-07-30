using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto);
    Task DeleteProductAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
}