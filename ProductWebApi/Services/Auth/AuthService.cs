using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductWebApi.DTO.auth;
using ProductWebApi.Models;

namespace ProductWebApi.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> Register(UserRegisterDto userRegisterDto)
    {
        bool exists = await _context.Users.AnyAsync(x => x.Username == userRegisterDto.Username);

        if (exists)
        {
            throw new Exception("tên của user đã tồn tại");
        }

        User user = new User
        {
            Username = userRegisterDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                userRegisterDto.Password),
            Role = userRegisterDto.Role
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        string token = GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponseDto> Login(UserDto userDto)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(x => x.Username == userDto.Username);

        if (user == null)
        {
            throw new Exception("Username hoặc Password không đúng");
        }

        bool passwordValid = BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash);

        if (!passwordValid)
        {
            throw new Exception("Username hoặc Password không đúng");
        }

        string token = GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            Role = user.Role.ToString()
        };
    }

    private string GenerateToken(User user)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        ];

        string? key = _configuration["Jwt:Key"];

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));

        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken token =
            new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}