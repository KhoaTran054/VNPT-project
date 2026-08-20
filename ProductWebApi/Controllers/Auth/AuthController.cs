using Microsoft.AspNetCore.Mvc;
using ProductWebApi.DTO.auth;
using ProductWebApi.Services.Auth;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(UserRegisterDto userRegisterDto)
    {
        AuthResponseDto result = await _authService.Register(userRegisterDto);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(UserDto userDto)
    {
        AuthResponseDto result = await _authService.Login(userDto);

        return Ok(result);
    }
}