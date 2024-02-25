using FluentValidation;
using WebForum.Application.Utils;

namespace WebForum.Application.Features.Auth.Login;

public class TwoFactorLoginCommandValidator : AbstractValidator<TwoFactorLoginCommand>
{
    public TwoFactorLoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.").MaximumLength(64)
            .Matches(@$"^[\d\w_.]+$").WithMessage("Username can only contain letter, digits, underscores and dots.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MaximumLength(128);
        RuleFor(x => x.TwoFactorCode)
            .NotEmpty().WithMessage("Authentication code is required.")
            .Length(Constants.UserAuthenticationCodeSize)
            .WithMessage($"Authentication code is a {Constants.UserAuthenticationCodeSize} digit number");
    }
}