using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProductWebApi.Models;

[Table("PRODUCT")]
public class Product
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("NAME")]
    [Required]
    public string Name { get; set; }
    
    [Column("PRICE")]
    public double Price { get; set; }
    
    [Column("QUANTITY")]
    public int Quantity { get; set; }
    
    [Column("CREATED_AT")]
    public DateTime CreatedAt { get; set; }
    
    [Column("CATEGORY_ID")]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
}   