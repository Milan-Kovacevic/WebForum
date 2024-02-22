namespace WebForum.Domain.Entities;
public class Topic
{
    public Guid TopicId { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}