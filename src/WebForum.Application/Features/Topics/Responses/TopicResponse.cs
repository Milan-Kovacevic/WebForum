namespace WebForum.Application.Features.Topics.Responses;

public class TopicResponse
{
    public Guid TopicId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}