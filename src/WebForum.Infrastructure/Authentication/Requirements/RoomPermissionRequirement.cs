using Microsoft.AspNetCore.Authorization;
using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication.Requirements;

public class RoomPermissionRequirement(RoomPermission permission) : IAuthorizationRequirement
{
    public RoomPermission Permission { get; } = permission;
}