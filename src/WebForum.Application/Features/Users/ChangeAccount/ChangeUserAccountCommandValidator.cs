using FluentValidation;
using WebForum.Domain.Enums;

namespace WebForum.Application.Features.Users.ChangeAccount;

public class ChangeUserAccountCommandValidator : AbstractValidator<ChangeUserAccountCommand>
{
    public ChangeUserAccountCommandValidator()
    {
        RuleFor(x => x.Role)
            .Must(BeValidUserRole)
            .WithMessage("Invalid user role specified.");
    }

    private static bool BeValidUserRole(UserRole? role)
    {
        return role is null || (Enum.IsDefined((UserRole)role) && role != UserRole.RootAdmin);
    }
}