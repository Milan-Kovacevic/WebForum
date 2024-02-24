using System.Security.Cryptography;
using WebForum.Application.Abstractions.Providers;

namespace WebForum.Infrastructure.Providers;

public class TwoFactorCodeProvider : ITwoFactorCodeProvider
{
    public Task<string> Generate2FaCode(int codeSize)
    {
        if (codeSize is < 0 or > 10)
            throw new InvalidOperationException();
        var endNumberExclusive = Math.Pow(10.0, codeSize);
        var codeValue = RandomNumberGenerator.GetInt32(0, (int)endNumberExclusive).ToString("D6");
        return Task.FromResult(codeValue);
    }
}