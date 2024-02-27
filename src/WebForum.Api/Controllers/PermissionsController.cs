using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Features.Permissions.Add;
using WebForum.Application.Features.Permissions.GetAll;
using WebForum.Application.Features.Permissions.GetForRoom;
using WebForum.Application.Features.Permissions.Remove;
using WebForum.Application.Requests;
using WebForum.Domain.Shared.Results;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class PermissionsController(ISender sender) : ApiController(sender)
{
    [AllowAnonymous]
    [HttpGet("[controller]")]
    public async Task<IActionResult> GetAllPermissions(CancellationToken cancellationToken)
    {
        return await Result.CreateFrom(new GetAllPermissionsQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [AllowAnonymous]
    [HttpGet("Users/{userId:guid}/Rooms/{roomId:guid}/[controller]")]
    public async Task<IActionResult> GetAllUserRoomPermissions([FromRoute] Guid userId, [FromRoute] Guid roomId,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetUserRoomPermissionsQuery(userId, roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [AllowAnonymous]
    [HttpPost("Users/{userId:guid}/Rooms/{roomId:guid}/[controller]/Add")]
    public async Task<IActionResult> AddUserRoomPermissions([FromRoute] Guid userId, [FromRoute] Guid roomId,
        [FromBody] ChangeUserPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new AddUserRoomPermissionCommand(userId, roomId, request.PermissionId))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }

    [AllowAnonymous]
    [HttpPost("Users/{userId:guid}/Rooms/{roomId:guid}/[controller]/Remove")]
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