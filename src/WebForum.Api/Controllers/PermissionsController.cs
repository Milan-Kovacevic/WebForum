using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Features.Permissions.Add;
using WebForum.Application.Features.Permissions.GetAll;
using WebForum.Application.Features.Permissions.GetForRoom;
using WebForum.Application.Features.Permissions.Remove;
using WebForum.Application.Requests;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Results;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class PermissionsController(ISender sender) : ApiController(sender)
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
    // TODO: Add handler and attribute to view own room permissions
    public async Task<IActionResult> GetAllUserRoomPermissions([FromRoute] Guid userId, [FromRoute] Guid roomId,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetUserRoomPermissionsQuery(userId, roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }
    
    [HttpPost("Users/{userId:guid}/Rooms/{roomId:guid}/[controller]/Add")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> AddUserRoomPermissions([FromRoute] Guid userId, [FromRoute] Guid roomId,
        [FromBody] ChangeUserPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new AddUserRoomPermissionCommand(userId, roomId, request.PermissionId))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
    
    [HttpPost("Users/{userId:guid}/Rooms/{roomId:guid}/[controller]/Remove")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> RemoveUserRoomPermissions([FromRoute] Guid userId, [FromRoute] Guid roomId,
        [FromBody] ChangeUserPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new RemoveUserRoomPermissionCommand(userId, roomId, request.PermissionId))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
}