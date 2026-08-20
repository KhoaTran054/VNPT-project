namespace ProductWebApi.DTO.Product;

public class ProductPagedDto
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public int? CategoryId { get; set; }

    public  ProductPriceSortOrder? SortPrice { get; set; }
}