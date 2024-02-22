using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Requests;
using WebForum.Application.Abstractions.Providers;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class AuthController(ISender sender, IJwtProvider jwtProvider) : ApiController(sender)
{
    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return Ok(await jwtProvider.GenerateUserToken(new User()
            {
                UserId = Guid.NewGuid(),
                Role = UserRole.Regular, 
                DisplayName = "Milan Kovacevic", 
                IsEnabled = true, 
                AccessFailedCount = 0,
                ConcurrencyStamp = Guid.NewGuid()
            }));
    }

    [HttpPost("/externalLogin")]
    public IResult Login([FromBody] string accessCode)
    {
        return Results.Challenge(authenticationSchemes: new List<string>() { "github" });
    }

    [HttpPost("/logout")]
    public async Task<IActionResult> Logout()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("/refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("/oauth/github")]
    public IActionResult GitHubCallback()
    {
        return Ok();
    }
}