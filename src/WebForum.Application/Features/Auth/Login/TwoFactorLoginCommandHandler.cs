using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Providers;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Auth.Responses;
using WebForum.Application.Utils;
using WebForum.Domain.Models.Errors;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Auth.Login;

public class TwoFactorLoginCommandHandler(
    IJwtProvider jwtProvider,
    IUserRepository userRepository,
    IUserTokenRepository userTokenRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<TwoFactorLoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(TwoFactorLoginCommand request, CancellationToken cancellationToken)
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

        //var twoFactorCode = await userTokenRepository.Get2FaCode(user.UserId, cancellationToken);
        //if (twoFactorCode is null || twoFactorCode.Value != request.TwoFactorCode)
        //    return Result.Failure<LoginResponse>(DomainErrors.User.InvalidLogin);
        //await userTokenRepository.Remove2FaCode(twoFactorCode, cancellationToken);

        var authTokens = await jwtProvider.GenerateUserToken(user);
        var response = new LoginResponse(authTokens.AccessToken, authTokens.RefreshToken, authTokens.ExpiresIn);
        return Result.Success(response);
    }
}