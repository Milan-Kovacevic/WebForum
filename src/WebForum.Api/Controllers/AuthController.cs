using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Api.Requests;
using WebForum.Application.Features.Auth.ExternalLogin;
using WebForum.Application.Features.Auth.Login;
using WebForum.Application.Features.Auth.Register;
using WebForum.Domain.Models.Extensions;
using WebForum.Domain.Models.Results;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class AuthController(ISender sender) : ApiController(sender)
{
    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await Result
            .CreateFrom(new RegisterCommand(request.DisplayName, request.Username, request.Email, request.Password))
            .Process(command => Sender.Send(command))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("login")]
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
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("/oauth/code")]
    public IResult GetOAuthCode()
    {
        return Results.Challenge(authenticationSchemes: new List<string>() { "github" });
    }
}