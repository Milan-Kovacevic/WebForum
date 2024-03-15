using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Infrastructure.Authentication.Requirements;

namespace WebForum.Infrastructure.Authentication.Handlers;

public class CommentOwnerAuthorizationHandler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<CommentOwnerAuthorizationHandler> logger) : AuthorizationHandler<CommentOwnerRequirement, Guid>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CommentOwnerRequirement requirement, Guid resource)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
        var jwtClaims = await jwtService.ExtractTokenClaimValues(context.User.Claims);

        if (jwtClaims is null)
        {
            logger.LogWarning(
                "Unable to extract token claims from {@Claims}.",
                context.User.Claims);
            context.Fail();
            return;
        }

        var commentRepository = scope.ServiceProvider.GetRequiredService<ICommentRepository>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        var commentOwner = (await commentRepository.GetByIdAsync(resource))?.UserId;
        if (commentOwner is null)
        {
            context.Fail();
            return;
        }

        var user = await userRepository.GetByIdAsync(jwtClaims.UserId);
        if (user is null)
            context.Fail();
        else if (requirement.ExcludedRoles.Any(r => r.RoleId == user.Role!.RoleId))
            context.Succeed(requirement);
        else if (requirement.RequiredRoles.Any(r => r.RoleId == user.Role!.RoleId) && commentOwner == user.UserId)
            context.Succeed(requirement);
        else context.Fail();
    }
}