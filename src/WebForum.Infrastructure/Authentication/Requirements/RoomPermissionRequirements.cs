using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication.Requirements;

public static class RoomPermissionRequirements
{
    public static readonly RoomPermissionRequirement CreateComment = new(RoomPermission.CreateComment);

    public static readonly RoomPermissionRequirement EditComment = new(RoomPermission.EditComment);

    public static readonly RoomPermissionRequirement RemoveComment = new(RoomPermission.RemoveComment);

    public static readonly RoomPermissionRequirement PostComment = new(RoomPermission.PostComment);

    public static readonly RoomPermissionRequirement BlockComment = new(RoomPermission.BlockComment);
}