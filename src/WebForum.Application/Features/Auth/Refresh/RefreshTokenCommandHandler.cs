using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Models;
using WebForum.Application.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Auth.Refresh;

public class RefreshTokenCommandHandler(IUserTokenRepository userTokenRepository, IJwtService jwtService)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var accessTokenValidationResult =
            await jwtService.ValidateUserToken(request.AccessToken, TokenType.Access);
        var refreshTokenValidationResult =
            await jwtService.ValidateUserToken(request.AccessToken, TokenType.Refresh);
        if (!accessTokenValidationResult.IsValid || !refreshTokenValidationResult.IsValid)
            return Result.Failure<RefreshTokenResponse>(DomainErrors.Auth.TokenExpired);

        var accessTokenClaims = await jwtService.ExtractClaimValues(accessTokenValidationResult.ClaimsIdentity.Claims);
        var refreshTokenClaims =
            await jwtService.ExtractClaimValues(refreshTokenValidationResult.ClaimsIdentity.Claims);

        if (accessTokenClaims is null || refreshTokenClaims is null ||
            accessTokenClaims.UserId != refreshTokenClaims.UserId)
            return Result.Failure<RefreshTokenResponse>(DomainErrors.Auth.InvalidToken);

        await userTokenRepository.RemoveUserToken(accessTokenClaims.UserId, accessTokenClaims.TokenId,
            accessTokenClaims.Type, cancellationToken);
        var removedRefreshToken = await userTokenRepository.RemoveUserToken(refreshTokenClaims.UserId,
            refreshTokenClaims.TokenId, refreshTokenClaims.Type, cancellationToken);
        if (removedRefreshToken is null)
            return Result.Failure<RefreshTokenResponse>(DomainErrors.Auth.InvalidToken);

        var authTokens = await jwtService.GenerateUserToken(removedRefreshToken.UserId);
        await userTokenRepository.PutUserToken(new UserToken()
        {
            Type = TokenType.Access,
            TokenId = authTokens.AccessTokenId,
            Value = authTokens.AccessToken,
            UserId = removedRefreshToken.UserId
        }, TimeSpan.FromSeconds(authTokens.AccessTokenExpiration), cancellationToken);
        await userTokenRepository.PutUserToken(new UserToken()
        {
            Type = TokenType.Refresh,
            TokenId = authTokens.RefreshTokenId,
            Value = authTokens.RefreshToken,
            UserId = removedRefreshToken.UserId
        }, TimeSpan.FromSeconds(authTokens.RefreshTokenExpiration), cancellationToken);

        var response = new RefreshTokenResponse(authTokens.AccessToken, authTokens.RefreshToken,
            authTokens.AccessTokenExpiration);
        return Result.Success(response);
    }

    private static TokenValidationOptions GetAccessTokenValidationOptions()
    {
        return new TokenValidationOptions()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateExpiration = false,
            ValidateSignature = true,
            IsAccessToken = true,
        };
    }

    private static TokenValidationOptions GetRefreshTokenValidationOptions()
    {
        return new TokenValidationOptions()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateExpiration = true,
            ValidateSignature = true,
            IsAccessToken = false,
        };
    }
}