using WebForum.Application.Models;
using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendSimpleEmail(SimpleEmail email, CancellationToken cancellationToken = default);
    Task Send2FaCodeEmail(string sendTo, TwoFactorCode code, CancellationToken cancellationToken = default);
}