using System.Security.Cryptography;
using WebForum.Application.Abstractions.Services;

namespace WebForum.Infrastructure.Services;

public class TwoFactorCodeService : ITwoFactorCodeService
{
    public Task<string> Generate2FaCode(int codeSize, CancellationToken cancellationToken = default)
    {
        if (codeSize is < 0 or > 10)
            throw new InvalidOperationException();
        var endNumberExclusive = Math.Pow(10.0, codeSize);
        var codeValue = RandomNumberGenerator.GetInt32(0, (int)endNumberExclusive).ToString("D6");
        return Task.FromResult(codeValue);
    }
}