using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Comments.Create;

public record CreateCommentCommand(Guid RoomId, Guid UserId, string Content) : ICommand<CommentResponse>
{
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate;
}