using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebForum.Api.Controllers;

[Route("/api/Rooms")]
public class CommentsController(ISender sender) : ApiController(sender)
{

    [HttpGet("{roomId:guid}/[controller]")]
    [AllowAnonymous]
    public IActionResult GetAllComments(Guid roomId)
    {
        return Ok(roomId);
    }
}