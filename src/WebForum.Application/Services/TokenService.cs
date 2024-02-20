using System.Security.Cryptography;
using WebForum.Application.Abstractions;
using WebForum.Domain.Entities;
using WebForum.Domain.Interfaces;

namespace WebForum.Application.Services;

public class TokenService(IUserTokenRepository userTokenRepository) : ITokenService
{
    private static readonly TimeSpan TwoFactorCodeDurationTime = TimeSpan.FromMinutes(2);

    public async Task<TwoFactorCode> Create2FaCode(Guid userId, CancellationToken cancellationToken = default)
    {
        var code = Generate2FaCode(userId);
        await userTokenRepository.Put2FaCode(code, cancellationToken);
        return code;
    }

    public async Task<bool> Verify2FaCode(Guid userId, string code, CancellationToken cancellationToken = default)
    {
        var savedCode = await userTokenRepository.Get2FaCode(userId, cancellationToken);
        return savedCode is not null && savedCode.Value == code && savedCode.UserId == userId;
    }

    private static TwoFactorCode Generate2FaCode(Guid userId)
    {
        var codeValue = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        return new TwoFactorCode()
            { UserId = userId, Value = codeValue, Duration = TwoFactorCodeDurationTime };
    }
}