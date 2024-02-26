using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Rooms.Update;

public record UpdateRoomCommand(Guid RoomId, string Name, string? Description) : ICommand<RoomResponse>
{
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate;
}