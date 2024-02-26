namespace WebForum.Application.Features.Topics.Responses;

public record RoomResponse(Guid TopicId, string Name, string? Description);