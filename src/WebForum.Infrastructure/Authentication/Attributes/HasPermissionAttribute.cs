using Microsoft.AspNetCore.Authorization;
using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication.Attributes;

public class HasPermissionAttribute(RoomPermission roomPermission)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public RoomPermission RoomPermission { get; } = roomPermission;
    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}