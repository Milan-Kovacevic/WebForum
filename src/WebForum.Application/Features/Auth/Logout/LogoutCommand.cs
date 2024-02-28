using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Auth.Logout;

public record LogoutCommand(Guid UserId) : ICommand
{
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Transaction;
}