namespace WebForum.Application.Features.Topics.Responses;

public record TopicResponse(Guid TopicId, string Name, string? Description);