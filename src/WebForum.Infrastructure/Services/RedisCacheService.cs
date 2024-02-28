using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using WebForum.Application.Abstractions.Services;

namespace WebForum.Infrastructure.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IServer _server;

    public RedisCacheService(IOptions<RedisCacheOptions> options)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(options.Value.Configuration!);
        _server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());
    }

    public IEnumerable<string> GetCacheKeys(string pattern)
    {
        var keys = _server.Keys(pattern: pattern);
        return keys.Select(x => x.ToString());
    }
}