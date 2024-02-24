using FluentValidation;
using WebForum.Domain.Enums;

namespace WebForum.Application.Features.Auth.ExternalLogin;

public class ExternalLoginCommandValidator : AbstractValidator<ExternalLoginCommand>
{
    public ExternalLoginCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Provider).NotEmpty().IsEnumName(typeof(LoginProvider));
    }
}