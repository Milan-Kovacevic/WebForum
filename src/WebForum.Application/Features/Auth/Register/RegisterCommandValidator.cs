using FluentValidation;

namespace WebForum.Application.Features.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Display name is required.")
            .MinimumLength(5).MaximumLength(128);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.")
            .MinimumLength(6).WithMessage("Username must be at least 6 characters long.").MaximumLength(64)
            .Matches(@$"^[\w-.]+$")
            .WithMessage("Username can only contain letter, digits, underscores and dots.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(128)
            .Must(password => password.Any(char.IsUpper) && password.Any(char.IsLower))
            .WithMessage("Password must contain at least one uppercase and lowercase letter.")
            .Must(password => password.Any(char.IsDigit)).WithMessage("Password must contain at least one digit")
            .Must(password => password.Any(c => !char.IsLetterOrDigit(c)))
            .WithMessage("Password must contain at least one special character");
    }
}