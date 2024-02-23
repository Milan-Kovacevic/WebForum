using FluentValidation;

namespace WebForum.Application.Features.Auth.Login;

public class TwoFactorLoginCommandValidator : LoginCommandValidator<TwoFactorLoginCommand>
{
    private const int TwoFactorCodeSize = 6;

    public TwoFactorLoginCommandValidator()
    {
        RuleFor(x => x.TwoFactorCode)
            .NotEmpty().WithMessage("Authentication code is required.")
            .Length(TwoFactorCodeSize).WithMessage("Authentication code is a 6 digit number");
    }
}