using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions;

public interface ITokenService
{
    Task<TwoFactorCode> Create2FaCode(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> Verify2FaCode(Guid userId, string code, CancellationToken cancellationToken = default);
}