using Microsoft.AspNetCore.Authorization;
using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication.Attributes;

public class HasPermissionAttribute(Permission permission)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public Permission Permission { get; } = permission;
    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}