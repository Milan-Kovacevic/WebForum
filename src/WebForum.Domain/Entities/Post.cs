using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class Post
{
    public Guid PostId { get; init; }
    public required string Content { get; init; }
    public required DateTime DatePosted { get; init; } 
    public required PostStatus Status { get; init; }
    public required DateTime DateUpdated { get; init; }
    public required Guid TopicId { get; init; }
    public Topic? Topic { get; init; }
    public required Guid UserId { get; init; }
    public User? User { get; init; }
}