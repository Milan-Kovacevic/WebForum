using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Providers;
using WebForum.Application.Features.Auth.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Domain.Models.Errors;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Auth.ExternalLogin;

public class ExternalLoginCommandHandler(IJwtProvider jwtProvider) : ICommandHandler<ExternalLoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (!Enum.TryParse<LoginProvider>(request.Provider, out var loginProvider))
            return Result.Failure<LoginResponse>(DomainErrors.Auth.InvalidExternalProvider);

        var user = new User()
        {
            Role = UserRole.Regular,
            UserId = Guid.NewGuid(),
            IsEnabled = true,
            AccessFailedCount = 0,
            DisplayName = "Test"
        };
        var authTokens = await jwtProvider.GenerateUserToken(user);
        var response = new LoginResponse(authTokens.AccessToken, authTokens.RefreshToken, authTokens.ExpiresIn);
        return Result.Success(response);
    }
}