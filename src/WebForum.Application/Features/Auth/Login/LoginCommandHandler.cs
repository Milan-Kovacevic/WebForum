using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Utils;
using WebForum.Domain.Entities;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Auth.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserTokenRepository userTokenRepository,
    IEmailService emailService,
    IAuthService authService)
    : ICommandHandler<LoginCommand>
{
    public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
            return Result.Failure(DomainErrors.Auth.InvalidLogin);

        if (user.LockoutEnd > DateTime.UtcNow || !user.IsEnabled)
            return Result.Failure(DomainErrors.Auth.InvalidLogin);

        var isValidPassword =
            user.PasswordHash is not null && authService.ValidatePasswordHash(request.Password, user.PasswordHash);

        if (!isValidPassword)
        {
            user.AccessFailedCount++;
            if (user.AccessFailedCount >= Constants.MaximumLoginAccessFailCount)
            {
                user.AccessFailedCount = 0;
                user.LockoutEnd = DateTime.UtcNow.Add(Constants.UserLockoutPeriod);
            }

            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Failure(DomainErrors.Auth.InvalidLogin);
        }

        if (user.Email is null)
            return Result.Failure(DomainErrors.Auth.InvalidLogin);

        var authenticationCode =
            await authService.Generate2FaCode(Constants.UserAuthenticationCodeSize, cancellationToken);
        var twoFactorCode = new TwoFactorCode()
        {
            UserId = user.UserId,
            Value = authenticationCode,
            Duration = Constants.UserAuthenticationCodeExpirationTime
        };
        await emailService.Send2FaCodeEmail(user.Email, twoFactorCode, cancellationToken);
        await userTokenRepository.Put2FaCode(twoFactorCode, cancellationToken);
        return Result.Success();
    }
}