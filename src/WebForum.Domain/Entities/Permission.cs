namespace WebForum.Domain.Entities;

public class Permission
{
    public Guid PermissionId { get; init; }
    public required string Name { get; init; }
}