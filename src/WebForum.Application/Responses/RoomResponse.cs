namespace WebForum.Application.Responses;

public record RoomResponse(Guid RoomId, string Name, string? Description);