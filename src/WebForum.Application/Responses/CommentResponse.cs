namespace WebForum.Application.Responses;

public record CommentResponse(
    Guid CommentId,
    string Content,
    DateTime DateCreate,
    DateTime? DateUpdated,
    DateTime? DatePosted,
    string UserDisplayName,
    string UserRoleName);