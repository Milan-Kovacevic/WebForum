using WebForum.Domain.Entities;
using WebForum.Domain.Enums;

namespace WebForum.Application.Abstractions.Repositories;

public interface IUserTokenRepository
{
    Task Put2FaCode(TwoFactorCode code, CancellationToken cancellationToken = default);
    Task<TwoFactorCode?> Get2FaCode(Guid userId, CancellationToken cancellationToken = default);
    Task Remove2FaCode(TwoFactorCode code, CancellationToken cancellationToken = default);
    Task PutUserToken(UserToken userToken, TimeSpan duration, CancellationToken cancellationToken = default);
    Task<UserToken?> GetUserToken(Guid userId, Guid tokenId, TokenType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserToken>> GetAllUserTokens(Guid userId, CancellationToken cancellationToken = default);
    Task RemoveAllUserTokens(Guid userId, CancellationToken cancellationToken = default);
}