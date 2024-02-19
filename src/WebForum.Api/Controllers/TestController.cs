using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebForum.Api.Configuration;

namespace WebForum.Api.Controllers;

[ApiController]
[Route("/api")]
public class TestController : ControllerBase
{
    [HttpGet("/test")]
    [EnableRateLimiting(Constants.RateLimiter.PolicyName)]
    public IActionResult HelloWorld()
    {
        return Ok("Hello world!");
    }
}