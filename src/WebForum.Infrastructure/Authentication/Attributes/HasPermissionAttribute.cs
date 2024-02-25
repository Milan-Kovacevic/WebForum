using Microsoft.AspNetCore.Authorization;

namespace WebForum.Infrastructure.Authentication.Attributes;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(policy: permission);