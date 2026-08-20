namespace ProductWebApi.DTO.auth;

public class UserRegisterDto
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Staff;
}