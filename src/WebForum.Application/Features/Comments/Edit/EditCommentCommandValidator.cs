using FluentValidation;
using WebForum.Application.Utils;

namespace WebForum.Application.Features.Comments.Edit;

public class EditCommentCommandValidator : AbstractValidator<EditCommentCommand>
{
    public EditCommentCommandValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();
        RuleFor(x => x.NewContent).NotEmpty().MaximumLength(Constants.MaximumCommentContentSize);
    }
}