using FluentValidation;
using WebForum.Application.Utils;

namespace WebForum.Application.Features.Comments.Create;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(Constants.MaximumCommentContentSize);
    }
}