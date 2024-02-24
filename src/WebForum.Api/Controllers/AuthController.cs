using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Api.Requests;
using WebForum.Application.Features.Auth.Login;
using WebForum.Application.Features.Auth.Register;
using WebForum.Domain.Models.Extensions;
using WebForum.Domain.Models.Results;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class AuthController(ISender sender) : ApiController(sender)
{
    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await Result
            .CreateFrom(new RegisterCommand(request.DisplayName, request.Username, request.Email, request.Password))
            .Process(command => Sender.Send(command))
            .Respond(Ok, HandleFailure);
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
            return await Result.CreateFrom(new LoginCommand(request.Username, request.Password))
                .Process(command => Sender.Send(command))
                .Respond(Ok, HandleFailure);
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