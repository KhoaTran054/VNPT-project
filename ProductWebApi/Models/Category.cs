using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProductWebApi.Models;

[Table("CATEGORY")]
public class Category
{
    [Column("ID")]
    [Key]
    public int  Id { get; set; }
    
    [Column("NAME")]
    [Required]
    public string Name { get; set; }
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}