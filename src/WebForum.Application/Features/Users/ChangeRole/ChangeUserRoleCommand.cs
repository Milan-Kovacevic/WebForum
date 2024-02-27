using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Domain.Enums;

namespace WebForum.Application.Features.Users.ChangeRole;

public record ChangeUserRoleCommand(Guid UserId, UserRole Role) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Sensitive | RequestFlag.Validate;
}