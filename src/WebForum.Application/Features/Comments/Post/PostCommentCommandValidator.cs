using FluentValidation;
using WebForum.Application.Utils;

namespace WebForum.Application.Features.Comments.Post;

public class PostCommentCommandValidator : AbstractValidator<PostCommentCommand>
{
    public PostCommentCommandValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();
        RuleFor(x => x.UpdatedContent).MaximumLength(Constants.MaximumCommentContentSize);
    }
}