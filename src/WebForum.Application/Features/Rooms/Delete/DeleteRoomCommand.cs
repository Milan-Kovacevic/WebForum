using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Rooms.Delete;

public record DeleteRoomCommand(Guid RoomId) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}