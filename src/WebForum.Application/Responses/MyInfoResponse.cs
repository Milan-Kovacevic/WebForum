using WebForum.Domain.Enums;

namespace WebForum.Application.Responses;

public record MyInfoResponse(Guid UserId, string DisplayName, UserRole Role);