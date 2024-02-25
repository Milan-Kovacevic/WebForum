namespace WebForum.Application.Abstractions.Services;

public interface ITwoFactorCodeService
{
    Task<string> Generate2FaCode(int codeSize, CancellationToken cancellationToken = default);
}