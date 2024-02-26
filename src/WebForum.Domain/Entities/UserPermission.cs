namespace WebForum.Domain.Entities;

public class UserPermission
{
    public required Guid UserId { get; init; }
    public required Guid RoomId { get; init; }
    public required int PermissionId { get; init; }

    public User? User { get; init; }
    public Room? Room { get; init; }
    public Permission? Permission { get; init; }
}