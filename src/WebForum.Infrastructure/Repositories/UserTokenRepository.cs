using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WebForum.Domain.Entities;
using WebForum.Domain.Interfaces;
using WebForum.Infrastructure.Configuration;

namespace WebForum.Infrastructure.Repositories;

public class UserTokenRepository(IDistributedCache distributedCache) : IUserTokenRepository
{
    public async Task Put2FaCode(TwoFactorCode code, CancellationToken cancellationToken = default)
    {
        var key = Resolve2FaCodeCacheKey(code.UserId);
        var value = JsonConvert.SerializeObject(code);
        
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = code.Duration
        };
        await distributedCache.SetStringAsync(key, value, options, cancellationToken);
    }

    public async Task<TwoFactorCode?> Get2FaCode(Guid userId, CancellationToken cancellationToken = default)
    {
        var key = Resolve2FaCodeCacheKey(userId);
        var value = await distributedCache.GetStringAsync(key, cancellationToken);
        return value is null ? null : JsonConvert.DeserializeObject<TwoFactorCode>(value);
    }

    private static string Resolve2FaCodeCacheKey(Guid userId)
    {
        return $"2fa:{userId}";
    }
}