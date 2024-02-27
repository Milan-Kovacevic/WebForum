using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Comments.Block;

public record BlockCommentCommand(Guid CommentId) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}