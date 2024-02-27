using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Permissions.Remove;

public record RemoveUserRoomPermissionCommand(Guid UserId, Guid RoomId, int PermissionId) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}