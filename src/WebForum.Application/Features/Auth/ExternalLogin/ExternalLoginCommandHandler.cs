using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Application.Features.Auth.ExternalLogin;

public class ExternalLoginCommandHandler(
    IUserRepository userRepository,
    IUserLoginRepository userLoginRepository,
    IUserTokenRepository userTokenRepository,
    IJwtService jwtService,
    IAuthService authService)
    : ICommandHandler<ExternalLoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<LoginProvider>(request.Provider, out var loginProvider))
            return Result.Failure<LoginResponse>(DomainErrors.Auth.InvalidExternalProvider);
        
        var authResult = await authService.ExternallyAuthenticateUser(loginProvider, request.Code);
        if (authResult.HasError || authResult.User is null)
            return Result.Failure<LoginResponse>(DomainErrors.Auth.OAuthInvalidLogin(authResult.ErrorMessage));

        var authUser = authResult.User;
        var userLogin = await userLoginRepository.GetByProviderAsync(loginProvider,
            authUser.ProviderId, cancellationToken);
        User user;
        if (userLogin is null)
        {
            user = new User()
            {
                RoleId = UserRole.Regular.RoleId,
                DisplayName = authUser.DisplayName,
                Username = null,
                PasswordHash = null,
                Email = null,
                IsEnabled = true,
                AccessFailedCount = 0,
            };
            await userRepository.InsertAsync(user, cancellationToken);
            
            userLogin = new UserLogin()
            {
                LoginProvider = loginProvider,
                ProviderKey = authUser.ProviderId,
                UserId = user.UserId
            };
            await userLoginRepository.InsertAsync(userLogin, cancellationToken);
        }
        else
            user = userLogin.User!;

        var authTokens = await jwtService.GenerateUserTokens(user.UserId);
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