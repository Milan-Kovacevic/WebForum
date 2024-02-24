using FluentValidation;

namespace WebForum.Application.Features.Auth.Login;

public class LoginCommandValidator<T> : AbstractValidator<T> where T : LoginCommand
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.").MaximumLength(64)
            .Matches(@$"^[\d\w_.]+$").WithMessage("Username can only contain letter, digits, underscores and dots.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MaximumLength(128);
    }
}