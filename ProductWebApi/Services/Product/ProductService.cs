using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductWebApi.DTO;
using ProductWebApi.DTO.Product;
using ProductWebApi.Models;

namespace ProductWebApi.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products.ToListAsync();

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<PagedResult<ProductDto>> GetAllPagedAsync(ProductPagedDto productPagedDto)
    {
        IQueryable<Product> products = _context.Products
            .Include(product => product.Category)
            .AsQueryable();

        if (productPagedDto.CategoryId.HasValue)
        {
            products = products.Where(product => 
                product.CategoryId == productPagedDto.CategoryId.Value);
        }

        if (productPagedDto.SortPrice == ProductPriceSortOrder.Asc)
        {
            products = products.OrderBy(p => p.Price);
        }
        else if (productPagedDto.SortPrice == ProductPriceSortOrder.Desc)
        {
            products = products.OrderByDescending(p => p.Price);
        }

        int totalItems = await products.CountAsync();

        var productList = await products
            .Skip(
                (productPagedDto.Page - 1) *
                productPagedDto.PageSize)
            .Take(productPagedDto.PageSize)
            .ToListAsync();

        List<ProductDto> items = _mapper.Map<List<ProductDto>>(productList);

        int totalPages = (int)Math.Ceiling((double)totalItems / productPagedDto.PageSize);

        PagedResult<ProductDto> result = new PagedResult<ProductDto>
        {
            Items = items,
            Page = productPagedDto.Page,
            PageSize = productPagedDto.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };

        return result;
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
    {
        var product = _mapper.Map<Product>(dto);

        product.CreatedAt = DateTime.UtcNow;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        _mapper.Map(dto, product);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product =
            await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}