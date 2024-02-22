using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Providers;

public interface IJwtProvider
{
    Task<string> GenerateUserToken(User user);
}