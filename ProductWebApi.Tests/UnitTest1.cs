using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductWebApi.DTO.Product;
using ProductWebApi.Mapper;
using ProductWebApi.Models;
using ProductWebApi.Services;

namespace ProductWebApi.Tests;

public class ProductServiceTests
{
    private ApplicationDbContext CreateContext()
    {
        DbContextOptions<ApplicationDbContext> options =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

        return new ApplicationDbContext(options);
    }

    private IMapper CreateMapper()
    {
        ILoggerFactory loggerFactory =
            LoggerFactory.Create(
                builder => { });

        MapperConfiguration configuration =
            new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<ProductMapper>();
                },
                loggerFactory);

        return configuration.CreateMapper();
    }
    
   [Fact]
    public async Task GetAllAsync_ReturnsProducts()
    {
        ApplicationDbContext context = CreateContext();

        context.Products.Add(
            new Product
            {
                Name = "áo polo",
                Price = 1000,
                Quantity = 10,
                CreatedAt = DateTime.UtcNow
            });

        context.Products.Add(
            new Product
            {
                Name = "áo gucci",
                Price = 50,
                Quantity = 20,
                CreatedAt = DateTime.UtcNow
            });

        await context.SaveChangesAsync();

        IMapper mapper = CreateMapper();

        ProductService service =
            new ProductService(context, mapper);

        List<ProductDto> result =
            await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ProductExists_ReturnsProduct()
    {
        ApplicationDbContext context = CreateContext();

        Product product = new Product
        {
            Name = "áo polo",
            Price = 1000,
            Quantity = 10,
            CreatedAt = DateTime.UtcNow
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        IMapper mapper = CreateMapper();

        ProductService service =
            new ProductService(context, mapper);

        ProductDto? result =
            await service.GetByIdAsync(product.Id);

        Assert.NotNull(result);
        Assert.Equal("áo polo", result.Name);
        Assert.Equal(1000, result.Price);
        Assert.Equal(10, result.Quantity);
    }

    [Fact]
    public async Task GetByIdAsync_ProductNotExists_ReturnsNull()
    {
        ApplicationDbContext context = CreateContext();

        IMapper mapper = CreateMapper();

        ProductService service =
            new ProductService(context, mapper);

        ProductDto? result =
            await service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CreatesProduct()
    {
        ApplicationDbContext context = CreateContext();

        IMapper mapper = CreateMapper();

        ProductService service =
            new ProductService(context, mapper);

        ProductCreateDto dto = new ProductCreateDto
        {
            Name = "áo polo",
            Price = 100,
            Quantity = 5,
            CategoryId = 1
        };

        ProductDto result =
            await service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("áo polo", result.Name);
        Assert.Equal(100, result.Price);
        Assert.Equal(5, result.Quantity);

        Product? product =
            await context.Products.FirstOrDefaultAsync();

        Assert.NotNull(product);
        Assert.Equal("áo polo", product.Name);
    }

    [Fact]
    public async Task UpdateAsync_ProductExists_ReturnsTrue()
    {
        ApplicationDbContext context = CreateContext();

        Product product = new Product
        {
            Name = "áo polo",
            Price = 100,
            Quantity = 5,
            CreatedAt = DateTime.UtcNow
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        IMapper mapper = CreateMapper();

        ProductService service =
            new ProductService(context, mapper);

        ProductUpdateDto dto = new ProductUpdateDto
        {
            Name = "áo gucci",
            Price = 200,
            Quantity = 10,
            CategoryId = 1
        };

        bool result =
            await service.UpdateAsync(product.Id, dto);

        Assert.True(result);

        Product? updatedProduct =
            await context.Products.FindAsync(product.Id);

        Assert.NotNull(updatedProduct);
        Assert.Equal("áo gucci", updatedProduct.Name);
        Assert.Equal(200, updatedProduct.Price);
        Assert.Equal(10, updatedProduct.Quantity);
    }

    [Fact]
    public async Task UpdateAsync_ProductNotExists_ReturnsFalse()
    {
        ApplicationDbContext context = CreateContext();

        IMapper mapper = CreateMapper();

        ProductService service =
            new ProductService(context, mapper);

        ProductUpdateDto dto = new ProductUpdateDto
        {
            Name = "áo polo",
            Price = 100,
            Quantity = 10,
            CategoryId = 1
        };

        bool result =
            await service.UpdateAsync(999, dto);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ProductExists_ReturnsTrue()
    {
        ApplicationDbContext context = CreateContext();

        Product product = new Product
        {
            Name = "áo polo",
            Price = 100,
            Quantity = 10,
            CreatedAt = DateTime.UtcNow
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        IMapper mapper = CreateMapper();

        ProductService service =
            new ProductService(context, mapper);

        bool result =
            await service.DeleteAsync(product.Id);

        Assert.True(result);

        Product? deletedProduct =
            await context.Products.FindAsync(product.Id);

        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteAsync_ProductNotExists_ReturnsFalse()
    {
        ApplicationDbContext context = CreateContext();

        IMapper mapper = CreateMapper();

        ProductService service =
            new ProductService(context, mapper);

        bool result =
            await service.DeleteAsync(999);

        Assert.False(result);
    }
}