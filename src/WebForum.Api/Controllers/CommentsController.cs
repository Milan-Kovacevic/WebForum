using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class CommentsController(ISender sender, IAuthorizationService authorizationService) : ApiController(sender)
{
    [HttpGet("Rooms/{roomId:guid}/[controller]/Posted")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator, UserRole.Regular)]
    public async Task<IActionResult> GetPostedComments(Guid roomId)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, roomId,
            new HasRoomPermissionRequirement(RoomPermission.CreateComment));

        return authorizationResult.Succeeded switch
        {
            false when User.Identity!.IsAuthenticated => Forbid(),
            false when !User.Identity!.IsAuthenticated => Challenge(),
            _ => Ok(roomId)
        };
    }
    
    [HttpGet("Rooms/{roomId:guid}/[controller]/Created")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator)]
    public async Task<IActionResult> GetAllCreatedComments(Guid roomId)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, roomId,
            new HasRoomPermissionRequirement(RoomPermission.CreateComment));

        return authorizationResult.Succeeded switch
        {
            false when User.Identity!.IsAuthenticated => Forbid(),
            false when !User.Identity!.IsAuthenticated => Challenge(),
            _ => Ok(roomId)
        };
    }

    [HttpPost("[controller]")]
    [HasRoomPermission(RoomPermission.CreateComment)]
    public async Task<IActionResult> CreateComment()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPut("[controller]/{commentId:guid}")]
    [HasRoomPermission(RoomPermission.EditComment)]
    public async Task<IActionResult> EditComment([FromRoute] Guid commentId)
    {
        await Task.CompletedTask;
        return Ok(commentId);
    }

    [HttpDelete("[controller]/{commentId:guid}")]
    [HasRoomPermission(RoomPermission.RemoveComment)]
    public async Task<IActionResult> RemoveComment([FromRoute] Guid commentId)
    {
        await Task.CompletedTask;
        return Ok(commentId);
    }

    [HttpPost("[controller]/{commentId:guid}/Post")]
    [HasRoomPermission(RoomPermission.PostComment)]
    public async Task<IActionResult> PostComment([FromRoute] Guid commentId)
    {
        await Task.CompletedTask;
        return Ok(commentId);
    }

    [HttpPost("[controller]/{commentId:guid}/Block")]
    [HasRoomPermission(RoomPermission.BlockComment)]
    public async Task<IActionResult> BlockComment([FromRoute] Guid commentId)
    {
        await Task.CompletedTask;
        return Ok(commentId);
    }
}