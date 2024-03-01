using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Auth.Logout;

public class LogoutCommandHandler(IUserRepository userRepository, IUserTokenRepository userTokenRepository)
    : ICommandHandler<LogoutCommand>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!await userRepository.ExistsByIdAsync(request.UserId, cancellationToken))
            return Result.Failure(DomainErrors.User.NotFound(request.UserId));
        
        await userTokenRepository.RemoveAllUserTokens(request.UserId, cancellationToken);
        return Result.Success();
    }
}