using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Responses;
using WebForum.Application.Utils;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Auth.Login;

public class TwoFactorLoginCommandHandler(
    IJwtService jwtService,
    IUserAuthService userAuthService,
    IUserRepository userRepository,
    IUserTokenRepository userTokenRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<TwoFactorLoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(TwoFactorLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
            return Result.Failure<LoginResponse>(DomainErrors.Auth.InvalidLogin);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<LoginResponse>(DomainErrors.Auth.InvalidLogin);

        // TODO: Check if user has social login...

        var isValidPassword =
            user.PasswordHash is not null && userAuthService.ValidatePasswordHash(request.Password, user.PasswordHash);
        var twoFactorCode = await userTokenRepository.Get2FaCode(user.UserId, cancellationToken);
        var isValidCode = twoFactorCode is not null && twoFactorCode.Value == request.TwoFactorCode;
        if (!isValidPassword || !isValidCode)
        {
            user.AccessFailedCount++;
            if (user.AccessFailedCount >= Constants.MaximumLoginAccessFailCount)
            {
                user.AccessFailedCount = 0;
                user.LockoutEnd = DateTime.UtcNow.Add(Constants.UserLockoutPeriod);
            }

            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Failure<LoginResponse>(DomainErrors.Auth.InvalidLogin);
        }

        if (user.Email is null)
            return Result.Failure<LoginResponse>(DomainErrors.Auth.InvalidLogin);
        
        await userTokenRepository.Remove2FaCode(twoFactorCode!, cancellationToken);

        user.AccessFailedCount = 0;
        var authTokens = await jwtService.GenerateUserToken(user.UserId);

        await userTokenRepository.PutUserToken(new UserToken()
        {
            Type = TokenType.Access,
            TokenId = authTokens.AccessTokenId,
            Value = authTokens.AccessToken,
            UserId = user.UserId,
            User = user
        }, TimeSpan.FromSeconds(authTokens.AccessTokenExpiration), cancellationToken);
        await userTokenRepository.PutUserToken(new UserToken()
        {
            Type = TokenType.Refresh,
            TokenId = authTokens.RefreshTokenId,
            Value = authTokens.RefreshToken,
            UserId = user.UserId,
            User = user
        }, TimeSpan.FromSeconds(authTokens.RefreshTokenExpiration), cancellationToken);

        var response = new LoginResponse(authTokens.AccessToken, authTokens.RefreshToken,
            authTokens.AccessTokenExpiration);
        return Result.Success(response);
    }
}