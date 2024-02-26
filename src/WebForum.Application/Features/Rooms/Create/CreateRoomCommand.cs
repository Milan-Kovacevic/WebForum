using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Rooms.Create;

public record CreateRoomCommand(string Name, string? Description) : ICommand<RoomResponse>
{
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate;
}