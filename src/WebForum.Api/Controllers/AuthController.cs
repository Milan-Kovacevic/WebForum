using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Requests;
using WebForum.Application.Features.Auth.Login;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class AuthController(ISender sender) : ApiController(sender)
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
        if (request.TwoFactorCode is not null)
        {
            var command = new TwoFactorLoginCommand(request.Username, request.Password, request.TwoFactorCode);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }
        else
        {
            var command = new LoginCommand(request.Username, request.Password);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }
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