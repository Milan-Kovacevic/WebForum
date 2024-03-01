using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
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
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var userPermissions = (await userRepository.GetByIdWithPermissionsAsync(userId))?.Permissions;

        if (userPermissions is not null && userPermissions
                .Any(p => p.Permission.Name == requirement.Permission.ToString() && p.RoomId == resource))
            context.Succeed(requirement);
        else
            context.Fail();
    }
}