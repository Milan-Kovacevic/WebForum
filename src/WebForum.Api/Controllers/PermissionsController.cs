using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Features.Permissions.Change;
using WebForum.Application.Features.Permissions.GetAll;
using WebForum.Application.Features.Permissions.GetForRoom;
using WebForum.Application.Requests;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Results;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class PermissionsController(ISender sender, IJwtService jwtService) : ApiController(sender)
{
    [HttpGet("[controller]")]
    public async Task<IActionResult> GetAllPermissions(CancellationToken cancellationToken)
    {
        return await Result.CreateFrom(new GetAllPermissionsQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }
    
    [HttpGet("Users/{userId:guid}/Rooms/{roomId:guid}/[controller]")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> GetAllUserRoomPermissions([FromRoute] Guid userId, [FromRoute] Guid roomId,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetUserRoomPermissionsQuery(userId, roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }
    
    [HttpGet("Rooms/{roomId:guid}/[controller]")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator, UserRole.Regular)]
    public async Task<IActionResult> GetUserRoomPermissions([FromRoute] Guid roomId,
        CancellationToken cancellationToken)
    {
        var tokenClaims = await jwtService.ExtractTokenClaimValues(User.Claims);
        if (tokenClaims is null)
            return Forbid();
        
        return await Result
            .CreateFrom(new GetUserRoomPermissionsQuery(tokenClaims.UserId, roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }
    
    [HttpPost("Users/{userId:guid}/Rooms/{roomId:guid}/[controller]")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> ChangeUserRoomPermissions([FromRoute] Guid userId, [FromRoute] Guid roomId,
        [FromBody] ChangeUserPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new ChangeUserRoomPermissionsCommand(userId, roomId, request.Permissions))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
}