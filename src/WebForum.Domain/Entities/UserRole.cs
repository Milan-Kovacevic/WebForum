using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class UserRole
{
    public int RoleId { get; init; }
    public required string Name { get; init; }

    public static readonly UserRole Regular = new()
    {
        RoleId = (int)Enums.UserRole.Regular,
        Name = Enums.UserRole.Regular.ToString()
    };

    public static readonly UserRole Moderator = new()
    {
        RoleId = (int)Enums.UserRole.Moderator,
        Name = Enums.UserRole.Moderator.ToString()
    };

    public static readonly UserRole Admin = new()
    {
        RoleId = (int)Enums.UserRole.Admin,
        Name = Enums.UserRole.Admin.ToString()
    };
    
    public static readonly UserRole RootAdmin = new()
    {
        RoleId = (int)Enums.UserRole.RootAdmin,
        Name = Enums.UserRole.RootAdmin.ToString()
    };
}