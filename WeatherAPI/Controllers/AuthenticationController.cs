using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using WeatherAPI.Models.Commands;
using WeatherAPI.Services.Interfaces;

namespace Medicalsoft.Controllers;

[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthenticationController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LogInUserCommand request)
    {
        var (success, content) = await _authService.Login(request.Email, request.Password);

        return !success ? Unauthorized("Wrong email or password") : Ok(content);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand request)
    {
        var user = await _authService.Register(request);

        if(user == null)
        {
            return BadRequest("Error while creating user");
        }

        return Ok(user);
    }
}