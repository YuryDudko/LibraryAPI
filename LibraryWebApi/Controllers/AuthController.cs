using Domain.Entities;
using LibraryWebApi.Services.TokenServices;
using LibraryWebApi.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public AuthController(UserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user, [FromQuery] string password)
    {
        bool isRegistered = await _userService.RegisterUserAsync(user, password);
        if (!isRegistered) return BadRequest("User already exists.");

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var dbUser = await _userService.AuthenticateUserAsync(user.Email, user.PasswordHash);
        if (dbUser == null) return Unauthorized("Invalid credentials");

        var accessToken = _tokenService.GenerateAccessToken(dbUser);
        var refreshToken = _tokenService.GenerateRefreshToken();
        dbUser.RefreshToken = refreshToken;
        dbUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userService.RegisterUserAsync(dbUser, dbUser.PasswordHash);

        return Ok(new { accessToken, refreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var user = await _userService.AuthenticateUserAsync(refreshToken, refreshToken);
        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return Unauthorized("Invalid or expired refresh token");

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        return Ok(new { accessToken = newAccessToken });
    }
}
