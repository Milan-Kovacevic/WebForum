using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Providers;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Features.Auth.Responses;
using WebForum.Application.Utils;
using WebForum.Domain.Entities;
using WebForum.Domain.Models.Errors;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Auth.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserTokenRepository userTokenRepository,
    ITwoFactorCodeProvider twoFactorCodeProvider,
    IEmailService emailService)
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsername(request.Username);
        if (user is null)
            return Result.Failure<LoginResponse>(DomainErrors.User.InvalidLogin);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<LoginResponse>(DomainErrors.User.InvalidLogin);

        // TODO: Check if user has social login...

        var isValidPassword =
            user.PasswordHash is not null && Utility.ValidateHash(request.Password, user.PasswordHash);

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
            return Result.Failure<LoginResponse>(DomainErrors.User.InvalidLogin);
        }

        if (user.Email is null)
            return Result.Failure<LoginResponse>(DomainErrors.User.InvalidLogin);

        var authenticationCode = await twoFactorCodeProvider.Generate2FaCode(Constants.UserAuthenticationCodeSize);
        var twoFactorCode = new TwoFactorCode()
        {
            UserId = user.UserId,
            Value = authenticationCode,
            Duration = Constants.UserAuthenticationCodeExpirationTime
        };
        await emailService.Send2FaCodeEmail(user.Email, twoFactorCode, cancellationToken);
        await userTokenRepository.Put2FaCode(twoFactorCode, cancellationToken);
        return Result.Success(new LoginResponse(string.Empty, string.Empty, 0));
    }
}