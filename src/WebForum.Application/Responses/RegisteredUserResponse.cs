using WebForum.Domain.Enums;

namespace WebForum.Application.Responses;

public record RegisteredUserResponse(Guid UserId, string DisplayName, bool IsEnabled, UserRole Role);