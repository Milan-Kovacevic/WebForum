using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Requests;
using WebForum.Application.Features.Auth.ExternalLogin;
using WebForum.Application.Features.Auth.Login;
using WebForum.Application.Features.Auth.Register;
using WebForum.Domain.Shared.Results;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class AuthController(ISender sender) : ApiController(sender)
{
    [HttpPost, Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await Result
            .CreateFrom(new RegisterCommand(request.DisplayName, request.Username, request.Email, request.Password))
            .Process(command => Sender.Send(command))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request.TwoFactorCode is not null)
        {
            var command = new TwoFactorLoginCommand(request.Username, request.Password, request.TwoFactorCode);
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }

        return await Result
            .CreateFrom(new LoginCommand(request.Username, request.Password))
            .Process(command => Sender.Send(command))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("externalLogin")]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLogin([FromBody] OAuthLoginRequest request)
    {
        return await Result
            .CreateFrom(new ExternalLoginCommand(request.Code, request.Provider))
            .Process(command => Sender.Send(command))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
    {
        await Task.CompletedTask;
        return Ok();
    }

    // Test
    [HttpGet("/oauth/code")]
    [AllowAnonymous]
    public IResult GetOAuthCode()
    {
        return Results.Challenge(authenticationSchemes: new List<string>() { "github" });
    }
}