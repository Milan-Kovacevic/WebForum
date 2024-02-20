using WebForum.Domain.Entities;

namespace WebForum.Domain.Interfaces;

public interface IUserTokenRepository
{
    Task Put2FaCode(TwoFactorCode code, CancellationToken cancellationToken = default);
    Task<TwoFactorCode?> Get2FaCode(Guid userId, CancellationToken cancellationToken = default);
}