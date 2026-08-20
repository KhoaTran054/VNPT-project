using ProductWebApi.DTO;
using ProductWebApi.DTO.auth;

namespace ProductWebApi.Services.Auth;

public interface IAuthService
{
    Task<AuthResponseDto> Register(UserRegisterDto registerDto);

    Task<AuthResponseDto> Login(UserDto loginDto);
}