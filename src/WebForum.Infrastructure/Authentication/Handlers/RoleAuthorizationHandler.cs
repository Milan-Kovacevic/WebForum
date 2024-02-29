using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using WebForum.Application.Abstractions.Services;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Infrastructure.Authentication.Handlers;

public class RoleAuthorizationHandler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<RoleAuthorizationHandler> logger) : AuthorizationHandler<HasRoleAttribute>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        HasRoleAttribute requirement)
    {
        var subject = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (subject is null || !Guid.TryParse(subject, out var userId))
        {
            logger.LogDebug(
                "User with claims {@Claims} does not have an id. Required user roles for authorized resource are {@Roles}",
                context.User.Claims, requirement.Roles);
            context.Fail();
            return;
        }

        // Injecting scoped service inside singleton...
        using var scope = serviceScopeFactory.CreateScope();
        var userAuthService = scope.ServiceProvider.GetRequiredService<IUserAuthService>();
        var role = await userAuthService.GetUserRole(userId);

        if (role is null || !requirement.UserRoles.Contains((UserRole)Enum.ToObject(typeof(UserRole), role.RoleId)))
        {
            logger.LogDebug(
                "User with claims {@Claims} does not have required roles {@Roles} for accessing resource. Current role {Role}",
                context.User.Claims, requirement.Roles, role);
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}