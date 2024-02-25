using Microsoft.AspNetCore.Authorization;
using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication.Attributes;

public class HasRoleAttribute(params UserRole[] roles)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public IEnumerable<UserRole> UserRoles { get; } = roles;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}