using System.ComponentModel.DataAnnotations;

namespace ProductWebApi.DTO.Product;

public class ProductCreateDto
{
    [Required(ErrorMessage = "tên của sản phẩm không được để trống")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "giá cả của sản phẩm phải lớn hơn 0")]
    public double Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "số lượng của sản phẩm phải lớn hơn hoặc bằng 0")]
    public int Quantity { get; set; }

    public int CategoryId { get; set; }
}