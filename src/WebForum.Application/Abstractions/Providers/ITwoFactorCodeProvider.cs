using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Providers;

public interface ITwoFactorCodeProvider
{
    Task<string> Generate2FaCode(int codeSize);
}