using Microsoft.AspNetCore.Mvc;

namespace WebForum.Api.Controllers;

[ApiController]
[Route("/api")]
public class TestController : ControllerBase
{
    [HttpGet("/test")]
    public IActionResult HelloWorld()
    {
        return Ok("Hello world!");
    }
}