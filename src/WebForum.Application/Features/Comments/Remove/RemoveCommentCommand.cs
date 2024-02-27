using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Comments.Remove;

public record RemoveCommentCommand(Guid CommentId) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}