using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Auth.Logout;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    public Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}