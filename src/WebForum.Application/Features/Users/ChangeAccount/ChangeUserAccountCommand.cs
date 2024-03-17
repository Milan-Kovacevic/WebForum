using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Domain.Enums;

namespace WebForum.Application.Features.Users.ChangeAccount;

public record ChangeUserAccountCommand(Guid UserId, UserRole? Role, bool? IsEnabled) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Sensitive | RequestFlag.Validate;
}