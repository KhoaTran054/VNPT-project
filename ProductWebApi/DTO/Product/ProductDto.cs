namespace ProductWebApi.DTO.Product;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public double Price { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CategoryId { get; set; }
}
