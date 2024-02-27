using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Infrastructure.Authentication.Providers;

public class RoomPermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy is not null || !Enum.TryParse<RoomPermission>(policyName, out var permission))
            return policy;
        
        return new AuthorizationPolicyBuilder()
            .AddRequirements(new HasRoomPermissionRequirement(permission))
            .Build();
    }
}