using Microsoft.AspNetCore.Authorization;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Infrastructure.Authentication.Handlers;

public class PermissionAuthorizationHandler : AuthorizationHandler<HasPermissionAttribute>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionAttribute requirement)
    {
        var permissions = requirement.Permissions;
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}