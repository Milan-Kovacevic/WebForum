using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Comments.Edit;

public record EditCommentCommand(Guid CommentId, string NewContent) : ICommand<CommentResponse>
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate;
}