using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Comments.Post;

public record PostCommentCommand(Guid CommentId, string? UpdatedContent) : ICommand<CommentResponse>
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}