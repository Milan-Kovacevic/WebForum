namespace WebForum.Domain.Entities;

public class Permission
{
    public int PermissionId { get; init; }
    public required string Name { get; init; }
}