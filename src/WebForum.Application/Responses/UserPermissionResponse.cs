namespace WebForum.Application.Responses;

public record UserPermissionResponse(int PermissionId, string PermissionName, Guid RoomId);