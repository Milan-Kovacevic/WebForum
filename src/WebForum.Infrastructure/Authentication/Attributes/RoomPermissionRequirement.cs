using Microsoft.AspNetCore.Authorization;
using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication.Attributes;

public class RoomPermissionRequirement(RoomPermission permission) : IAuthorizationRequirement
{
    public RoomPermission Permission { get; } = permission;
}