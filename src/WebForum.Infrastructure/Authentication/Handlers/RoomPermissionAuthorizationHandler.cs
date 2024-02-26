using Microsoft.AspNetCore.Authorization;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Infrastructure.Authentication.Handlers;

public class RoomPermissionAuthorizationHandler : AuthorizationHandler<HasRoomPermissionAttribute, Guid>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        HasRoomPermissionAttribute requirement, Guid resource)
    {
        var permission = requirement.RoomPermission;
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}