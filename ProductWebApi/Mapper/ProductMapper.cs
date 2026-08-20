using AutoMapper;
using ProductWebApi.Models;
using ProductWebApi.DTO;
using ProductWebApi.DTO.Product;

namespace ProductWebApi.Mapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();
    }
}