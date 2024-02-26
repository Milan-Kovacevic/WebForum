using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api/Rooms/{roomId:guid}/[controller]")]
public class CommentsController(ISender sender, IAuthorizationService authorizationService) : ApiController(sender)
{
    [HttpGet]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator, UserRole.Regular)]
    [HasRoomPermission(RoomPermission.CreateComment)]
    public async Task<IActionResult> GetComments(Guid roomId)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, roomId, RoomPermission.CreateComment.ToString());

        return authorizationResult.Succeeded switch
        {
            false when User.Identity!.IsAuthenticated => Forbid(),
            false when !User.Identity!.IsAuthenticated => Challenge(),
            _ => Ok(roomId)
        };
    }
}