using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductWebApi.DTO;

namespace ProductWebApi.Models;

[Table("USERS")]
public class User
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("USERNAME")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("PASSWORD_HASH")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [Column("ROLE")]
    public UserRole Role { get; set; }
}