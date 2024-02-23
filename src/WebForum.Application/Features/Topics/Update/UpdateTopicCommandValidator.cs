using FluentValidation;

namespace WebForum.Application.Features.Topics.Update;

public class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
{
    public UpdateTopicCommandValidator()
    {
        RuleFor(x => x.TopicId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Description).MaximumLength(128);
    }
}