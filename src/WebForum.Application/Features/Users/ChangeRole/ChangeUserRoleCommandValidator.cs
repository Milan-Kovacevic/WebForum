using FluentValidation;
using WebForum.Domain.Enums;

namespace WebForum.Application.Features.Users.ChangeRole;

public class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleCommandValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(BeValidUserRole)
            .WithMessage("Invalid user role specified.");
    }

    private static bool BeValidUserRole(UserRole role)
    {
        return Enum.IsDefined(role) && role != UserRole.RootAdmin;
    }
}