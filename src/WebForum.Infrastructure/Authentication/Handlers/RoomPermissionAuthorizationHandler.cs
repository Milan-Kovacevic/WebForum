using Microsoft.AspNetCore.Authorization;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Infrastructure.Authentication.Handlers;

public class RoomPermissionAuthorizationHandler : AuthorizationHandler<HasRoomPermissionRequirement, Guid>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        HasRoomPermissionRequirement requirement, Guid resource)
    {
        var permission = requirement.Permission;
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}