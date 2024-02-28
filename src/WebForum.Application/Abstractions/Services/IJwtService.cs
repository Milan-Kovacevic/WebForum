using WebForum.Application.Models;
using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Services;

public interface IJwtService
{
    Task<AuthToken> GenerateUserToken(User user);
}