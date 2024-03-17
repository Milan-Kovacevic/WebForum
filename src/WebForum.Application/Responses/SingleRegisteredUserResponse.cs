using System.Collections;
using WebForum.Domain.Enums;

namespace WebForum.Application.Responses;

public record SingleRegisteredUserResponse(
    Guid UserId,
    string DisplayName,
    bool IsEnabled,
    int RoleId,
    bool IsExternallyAuthenticated,
    IEnumerable<UserPermissionResponse> Permissions)
    : RegisteredUserResponse(UserId, DisplayName, IsEnabled, RoleId);