using Microsoft.AspNetCore.Authorization;
using WebForum.Domain.Entities;

namespace WebForum.Infrastructure.Authentication.Requirements;

public class CommentOwnerRequirement(IEnumerable<UserRole> requiredRoles, IEnumerable<UserRole> excludedRoles)
    : IAuthorizationRequirement
{
    public IEnumerable<UserRole> RequiredRoles { get; } = requiredRoles;
    public IEnumerable<UserRole> ExcludedRoles { get; } = excludedRoles;
}