using ProductWebApi.DTO;
using ProductWebApi.DTO.Product;

namespace ProductWebApi.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    
    Task<PagedResult<ProductDto>> GetAllPagedAsync(ProductPagedDto productPagedDto);

    Task<ProductDto?> GetByIdAsync(int id);

    Task<ProductDto> CreateAsync(ProductCreateDto dto);

    Task<bool> UpdateAsync(int id, ProductUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}