namespace WebForum.Domain.Entities;
public class Topic
{
    public Guid TopicId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}