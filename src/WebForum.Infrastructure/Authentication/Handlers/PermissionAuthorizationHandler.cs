using Microsoft.AspNetCore.Authorization;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Infrastructure.Authentication.Handlers;

public class PermissionAuthorizationHandler : AuthorizationHandler<HasPermissionAttribute>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionAttribute requirement)
    {
        var permission = requirement.RoomPermission;
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}