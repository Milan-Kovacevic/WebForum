using System.IdentityModel.Tokens.Jwt;
using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Responses;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Auth.Refresh;

public class RefreshTokenCommandHandler(IUserTokenRepository userTokenRepository, IJwtService jwtService)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // TODO..
        var handler = new JwtSecurityTokenHandler();
        handler.ReadJwtToken(request.AccessToken);
        throw new NotImplementedException();
    }
}