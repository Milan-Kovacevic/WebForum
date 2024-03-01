using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;

namespace WebForum.Persistence.Repositories;

public class UserTokenRepository(IDistributedCache distributedCache, IRedisCacheService redisCacheService)
    : IUserTokenRepository
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

    public async Task Remove2FaCode(TwoFactorCode code, CancellationToken cancellationToken = default)
    {
        var key = Resolve2FaCodeCacheKey(code.UserId);
        await distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task PutUserToken(UserToken userToken, TimeSpan duration,
        CancellationToken cancellationToken = default)
    {
        var key = ResolveUserTokenCacheKey(userToken.UserId, userToken.TokenId, userToken.Type);
        var value = JsonConvert.SerializeObject(userToken);

        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = duration
        };
        await distributedCache.SetStringAsync(key, value, options, cancellationToken);
    }

    public async Task<UserToken?> GetUserToken(Guid userId, Guid tokenId, TokenType type,
        CancellationToken cancellationToken = default)
    {
        var key = ResolveUserTokenCacheKey(userId, tokenId, type);
        return await GetToken(key, cancellationToken);
    }

    public async Task<IEnumerable<UserToken>> GetAllUserTokens(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var keys = redisCacheService.GetCacheKeys(ResolveUserTokensPattern(userId));
        var resolvedTokenTasks = keys.Select(key => GetToken(key, cancellationToken)).ToList();
        var tokens = await Task.WhenAll(resolvedTokenTasks);
        return tokens.Where(x => x is not null).Select(x => x!);
    }

    public async Task<UserToken?> RemoveUserToken(Guid userId, Guid tokenId, TokenType type,
        CancellationToken cancellationToken)
    {
        var key = ResolveUserTokenCacheKey(userId, tokenId, type);
        var userToken = await GetUserToken(userId, tokenId, type, cancellationToken);
        if (userToken is null)
            return null;
        await distributedCache.RemoveAsync(key, cancellationToken);
        return userToken;
    }

    public async Task RemoveAllUserTokens(Guid userId, CancellationToken cancellationToken = default)
    {
        var keys = redisCacheService.GetCacheKeys(ResolveUserTokensPattern(userId));
        var removedTokenTasks = keys.Select(key => distributedCache.RemoveAsync(key, cancellationToken)).ToList();
        await Task.WhenAll(removedTokenTasks);
    }

    private async Task<UserToken?> GetToken(string key, CancellationToken cancellationToken = default)
    {
        var value = await distributedCache.GetStringAsync(key, cancellationToken);
        return value is null ? default : JsonConvert.DeserializeObject<UserToken>(value);
    }

    private static string Resolve2FaCodeCacheKey(Guid userId)
    {
        return $"2fa:{userId}";
    }

    private static string ResolveUserTokenCacheKey(Guid userId, Guid tokenId, TokenType type)
    {
        return $"token:{userId}:{(type == TokenType.Access ? "access" : "refresh")}:{tokenId}";
    }

    private static string ResolveUserTokensPattern(Guid userId)
    {
        return $"token:{userId}:*";
    }
}