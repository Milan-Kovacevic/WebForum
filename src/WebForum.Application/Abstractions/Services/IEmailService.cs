using WebForum.Domain.Entities;
using WebForum.Domain.Models;

namespace WebForum.Application.Abstractions.Services;

public interface IEmailService
{
    Task Send2FaCodeEmail(string sendTo, TwoFactorCode code, CancellationToken cancellationToken = default);
}