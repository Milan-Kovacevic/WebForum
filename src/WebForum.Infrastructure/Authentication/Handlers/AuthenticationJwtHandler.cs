using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Abstractions.Services;

namespace WebForum.Infrastructure.Authentication.Handlers;

public class AuthenticationJwtHandler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<AuthenticationJwtHandler> logger)
    : JsonWebTokenHandler
{
    public override async Task<TokenValidationResult> ValidateTokenAsync(SecurityToken token,
        TokenValidationParameters validationParameters)
    {
        var validationResult = await base.ValidateTokenAsync(token, validationParameters);
        if (!validationResult.IsValid || token is not JsonWebToken jwt)
            return validationResult;

        using var scope = serviceScopeFactory.CreateScope();
        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
        var userAuthService = scope.ServiceProvider.GetRequiredService<IUserAuthService>();

        var claimValues = await jwtService.ExtractClaimValuesFromJwt(jwt);
        if (claimValues is null)
        {
            logger.LogWarning(
                "Required claims cannot be extracted from jwt {Token} with claims {@Claims}.",
                jwt.EncodedToken, jwt.Claims);
            validationResult.IsValid = false;
            return validationResult;
        }

        if (claimValues.Type != Domain.Enums.TokenType.Access)
        {
            logger.LogWarning(
                "Authentication failed with refresh token. Extracted claims are {@ClaimValues}. Encoded token is {Token}",
                claimValues, jwt.EncodedToken);
            validationResult.IsValid = false;
            return validationResult;
        }

        var userToken = await userAuthService.GetUserToken(claimValues.UserId, claimValues.TokenId, claimValues.Type);
        if (userToken is null)
        {
            logger.LogInformation("User token was not found in the database. Encoded token is {Token}",
                jwt.EncodedToken);
        }

        return validationResult;
    }
}