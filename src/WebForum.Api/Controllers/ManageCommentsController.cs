using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class ManageCommentsController(ISender sender) : ApiController(sender)
{
    [HttpPost("{commentId:guid}/Post")]
    public async Task<IActionResult> PostComment([FromRoute] Guid commentId)
    {
        await Task.CompletedTask;
        return Ok(commentId);
    }
    
    [HttpPost("{commentId:guid}/Block")]
    public async Task<IActionResult> BlockComment([FromRoute] Guid commentId)
    {
        await Task.CompletedTask;
        return Ok(commentId);
    }
    
    [HttpGet("Rooms/{roomId:guid}")]
    public async Task<IActionResult> GetRoomComments([FromRoute] Guid roomId)
    {
        await Task.CompletedTask;
        return Ok(roomId);
    }
}