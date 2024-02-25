namespace WebForum.Domain.Entities;

public class UserPermission
{
    public required Guid UserId { get; init; }
    public required Guid PermissionId { get; init; }
    public required Guid TopicId { get; init; }

    public User? User { get; init; }
    public Permission? Permission { get; init; }
    public Topic? Topic { get; init; }
}