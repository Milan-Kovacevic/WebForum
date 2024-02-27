using System.Collections;
using WebForum.Domain.Enums;

namespace WebForum.Application.Responses;

public record SingleRegisteredUserResponse(
    Guid UserId,
    string DisplayName,
    bool IsEnabled,
    int RoleId,
    string GroupName,
    bool IsExternallyAuthenticated,
    IEnumerable<UserPermissionResponse> Permissions)
    : RegisteredUserResponse(UserId, DisplayName, IsEnabled, RoleId, GroupName);