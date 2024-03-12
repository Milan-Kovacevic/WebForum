namespace WebForum.Application.Requests;

public record CreateCommentRequest(Guid RoomId, string Content);