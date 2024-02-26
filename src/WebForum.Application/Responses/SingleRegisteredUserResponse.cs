using System.Collections;
using WebForum.Domain.Enums;

namespace WebForum.Application.Responses;

public record SingleRegisteredUserResponse(
    Guid UserId,
    string DisplayName,
    bool IsEnabled,
    UserRole Role,
    bool IsExternallyAuthenticated,
    IEnumerable<PermissionResponse> Permissions)
    : RegisteredUserResponse(UserId, DisplayName, IsEnabled, Role);