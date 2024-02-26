using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class Comment
{
    public Guid CommentId { get; init; }
    public required string Content { get; set; }
    public required DateTime DateCreated { get; set; } 
    public DateTime? DateUpdated { get; set; }
    public DateTime? DatePosted { get; set; }
    public required CommentStatus Status { get; set; }
    public required Guid RoomId { get; set; }
    public required Guid UserId { get; set; }
}