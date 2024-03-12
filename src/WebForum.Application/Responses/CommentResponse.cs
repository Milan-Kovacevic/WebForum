namespace WebForum.Application.Responses;

public record CommentResponse(
    Guid CommentId,
    string Content,
    DateTime DateCreated,
    DateTime? DateUpdated,
    DateTime? DatePosted,
    Guid UserId,
    string UserDisplayName,
    int RoleId);