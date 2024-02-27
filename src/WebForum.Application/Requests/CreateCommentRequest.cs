namespace WebForum.Application.Requests;

public record CreateCommentRequest(Guid UserId, Guid RoomId, string Content);