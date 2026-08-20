using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductWebApi.DTO;
using ProductWebApi.DTO.Product;
using ProductWebApi.Models;
using ProductWebApi.Services;

namespace ProductWebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("/getAll")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var listProduct = await _productService.GetAllAsync();

        return Ok(listProduct);
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts([FromQuery] ProductPagedDto productPagedDto)
    {
        if (productPagedDto.Page < 1)
        {
            productPagedDto.Page = 1;
        }

        if (productPagedDto.PageSize < 1)
        {
            productPagedDto.PageSize = 10;
        }

        if (productPagedDto.PageSize > 100)
        {
            productPagedDto.PageSize = 100;
        }

        PagedResult<ProductDto> result = await _productService.GetAllPagedAsync(productPagedDto);

        return Ok(result);
    }

    [HttpGet("/getById/{id}", Name = "getById")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound(new { message = $"Không tìm thấy sản phẩm có Id = {id}" });
        }

        return Ok(product);
    }
    
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto productCreateDto)
    {
        var result = await _productService.CreateAsync(productCreateDto);

        return CreatedAtRoute("getById", new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
    {
        bool updated = await _productService.UpdateAsync(id, productUpdateDto);

        if (!updated)
        {
            return NotFound(new { message = $"Không tìm thấy sản phẩm có Id = {id}" });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        bool deleted = await _productService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new { message = $"Không tìm thấy sản phẩm có Id = {id}" });
        }

        return NoContent();
    }
}