using WebForum.Domain.Enums;

namespace WebForum.Application.Requests;

public record ChangeUserAccountRequest(UserRole? Role, bool? IsEnabled);