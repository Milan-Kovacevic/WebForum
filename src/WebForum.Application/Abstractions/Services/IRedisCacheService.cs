namespace WebForum.Application.Abstractions.Services;

public interface IRedisCacheService
{
    IEnumerable<string> GetCacheKeys(string pattern);
}