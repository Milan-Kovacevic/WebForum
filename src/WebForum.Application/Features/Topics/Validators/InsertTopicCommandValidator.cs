using FluentValidation;
using WebForum.Application.Features.Topics.Commands;

namespace WebForum.Application.Features.Topics.Validators;

public class InsertTopicCommandValidator : AbstractValidator<InsertTopicCommand>
{
    public InsertTopicCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Description).MaximumLength(128);
    }
}