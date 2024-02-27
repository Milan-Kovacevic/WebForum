using WebForum.Domain.Enums;

namespace WebForum.Application.Requests;

public record ChangeUserGroupRequest(UserRole Role);