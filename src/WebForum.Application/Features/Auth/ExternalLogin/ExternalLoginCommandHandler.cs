using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Auth.ExternalLogin;

public class ExternalLoginCommandHandler(IJwtService jwtService) : ICommandHandler<ExternalLoginCommand, LoginResponse>
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
        var authTokens = await jwtService.GenerateUserToken(user);
        var response = new LoginResponse(authTokens.AccessToken, authTokens.RefreshToken, authTokens.ExpiresIn);
        return Result.Success(response);
    }
}