using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Permissions.Change;

public record ChangeUserRoomPermissionsCommand(Guid UserId, Guid RoomId, int[] Permissions) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}