using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Services;

public interface IEmailService
{
    Task Send2FaCodeEmail(string sendTo, TwoFactorCode code, CancellationToken cancellationToken = default);
}