namespace WebForum.Domain.Entities;

public class UserPermission
{
    public required Guid UserId { get; init; }
    public required Guid PermissionId { get; init; }
    public required Guid RoomId { get; init; }

    public User? User { get; init; }
    public Permission? Permission { get; init; }
    public Room? Room { get; init; }
}