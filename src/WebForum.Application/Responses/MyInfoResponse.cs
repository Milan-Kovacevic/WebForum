using WebForum.Domain.Enums;

namespace WebForum.Application.Responses;

public record MyInfoResponse(string DisplayName, UserRole Role);