using FluentValidation;
using WebForum.Application.Utils;

namespace WebForum.Application.Features.Auth.Login;

public class TwoFactorLoginCommandValidator : LoginCommandValidator<TwoFactorLoginCommand>
{
    public TwoFactorLoginCommandValidator()
    {
        RuleFor(x => x.TwoFactorCode)
            .NotEmpty().WithMessage("Authentication code is required.")
            .Length(Constants.UserAuthenticationCodeSize)
            .WithMessage($"Authentication code is a {Constants.UserAuthenticationCodeSize} digit number");
    }
}