using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication.Attributes;

public static class RoomPermissionRequirements
{
    public static readonly RoomPermissionRequirement CreateComment =
        new RoomPermissionRequirement(RoomPermission.CreateComment);

    public static readonly RoomPermissionRequirement EditComment =
        new RoomPermissionRequirement(RoomPermission.EditComment);

    public static readonly RoomPermissionRequirement RemoveComment =
        new RoomPermissionRequirement(RoomPermission.RemoveComment);

    public static readonly RoomPermissionRequirement PostComment =
        new RoomPermissionRequirement(RoomPermission.PostComment);

    public static readonly RoomPermissionRequirement BlockComment =
        new RoomPermissionRequirement(RoomPermission.BlockComment);
}