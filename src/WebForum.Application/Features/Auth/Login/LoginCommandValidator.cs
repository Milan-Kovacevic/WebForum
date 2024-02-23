using FluentValidation;

namespace WebForum.Application.Features.Auth.Login;

public class LoginCommandValidator<T> : AbstractValidator<T> where T: LoginCommand
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.")
            .MinimumLength(6).WithMessage("Username must be at least 6 characters long.").MaximumLength(64)
            .Matches(@$"^[\d\w_.]+$")
            .WithMessage("Username can only contain letter, digits, underscores and dots.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(128)
            .Must(password => password.Any(char.IsUpper) && password.Any(char.IsLower))
            .WithMessage("Password must contain at least one uppercase and lowercase letter.")
            .Must(password => password.Any(char.IsDigit)).WithMessage("Password must contain at least one digit")
            .Must(password => password.Any(c => !char.IsLetterOrDigit(c)));
    }
}