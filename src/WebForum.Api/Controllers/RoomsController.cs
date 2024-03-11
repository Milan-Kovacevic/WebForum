using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Requests;
using WebForum.Application.Features.Rooms.Create;
using WebForum.Application.Features.Rooms.Delete;
using WebForum.Application.Features.Rooms.GetAll;
using WebForum.Application.Features.Rooms.GetById;
using WebForum.Application.Features.Rooms.Update;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Results;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class RoomsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllRooms(CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetAllRoomsQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpGet("{roomId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoomById(Guid roomId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetRoomByIdQuery(roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> CreateRoom([FromBody] RoomRequest request, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new CreateRoomCommand(request.Name, request.Description))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPut("{roomId:guid}")]
    [HasRole(UserRole.RootAdmin)]
    public async Task<IActionResult> UpdateRoom(Guid roomId, [FromBody] RoomRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new UpdateRoomCommand(roomId, request.Name, request.Description))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpDelete("{roomId:guid}")]
    [HasRole(UserRole.RootAdmin)]
    public async Task<IActionResult> DeleteRoom(Guid roomId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new DeleteRoomCommand(roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
}