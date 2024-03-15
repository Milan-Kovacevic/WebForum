namespace WebForum.Application.Responses;

public record RoomResponse(Guid RoomId, string Name, DateTime DateCreated, string? Description);