using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebForum.Api.Configuration;
using WebForum.Application.Abstractions.Messaging;
using WebForum.Application.Abstractions.Services;

namespace WebForum.Api.Controllers;

[ApiController]
[Route("/api")]
public class TestController(ITokenService tokenService, IEmailService emailService, IGitHubClient gitHubClient) : ControllerBase
{
    [HttpGet("/code/generate")]
    [EnableRateLimiting(Constants.RateLimiter.PolicyName)]
    public async Task<IActionResult> GenerateCode(CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        var code = await tokenService.Create2FaCode(userId, cancellationToken);
        await emailService.Send2FaCodeEmail("test@test.com", code, cancellationToken);
        return Ok($"{userId}, Check your email for authentication code");
    }

    [HttpGet("/code/verify")]
    [EnableRateLimiting(Constants.RateLimiter.PolicyName)]
    public async Task<IActionResult> VerifyCode([FromQuery] Guid userId, [FromQuery] string code,
        CancellationToken cancellationToken)
    {
        var result = await tokenService.Verify2FaCode(userId, code, cancellationToken);
        return Ok($"Verification result : {result}");
    }

    [HttpGet("/github")]
    public async Task<IActionResult> GetGithubUser([FromQuery] string username)
    {
        var result = await gitHubClient.GetUserInfo(username);
        return Ok(result);
    }
}