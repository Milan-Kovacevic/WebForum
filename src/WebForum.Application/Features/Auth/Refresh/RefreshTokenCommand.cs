using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Auth.Refresh;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : ICommand<RefreshTokenResponse>
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}