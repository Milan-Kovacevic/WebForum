using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Infrastructure.Authentication.Handlers;

public class RoomPermissionAuthorizationHandler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<RoomPermissionAuthorizationHandler> logger) : AuthorizationHandler<RoomPermissionRequirement, Guid>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        RoomPermissionRequirement requirement, Guid resource)
    {
        var subject = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (subject is null || !Guid.TryParse(subject, out var userId))
        {
            logger.LogDebug(
                "User does not have an id in his claims {@Claims}.",
                context.User.Claims);
            context.Fail();
            return;
        }

        using var scope = serviceScopeFactory.CreateScope();
        var userAuthService = scope.ServiceProvider.GetRequiredService<IUserAuthService>();
        var userPermissions = await userAuthService.GetUserRoomPermissions(userId, resource);

        if (userPermissions.Any(p => p.Name == requirement.Permission.ToString()))
            context.Succeed(requirement);
        else
            context.Fail();
    }
}