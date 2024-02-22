using FluentValidation;

namespace WebForum.Application.Features.Topics.Create;

public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Description).MaximumLength(128);
    }
}