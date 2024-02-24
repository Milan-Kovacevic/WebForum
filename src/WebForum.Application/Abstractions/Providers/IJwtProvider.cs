using WebForum.Domain.Entities;
using WebForum.Domain.Models;

namespace WebForum.Application.Abstractions.Providers;

public interface IJwtProvider
{
    Task<AuthTokens> GenerateUserToken(User user);
}