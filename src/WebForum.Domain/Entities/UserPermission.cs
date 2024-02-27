namespace WebForum.Domain.Entities;

public class UserPermission
{
    public required Guid UserId { get; init; }
    public required Guid RoomId { get; init; }
    public required int PermissionId { get; init; }

    public required User User { get; init; }
    public required Room Room { get; init; }
    public required Permission Permission { get; init; }
}