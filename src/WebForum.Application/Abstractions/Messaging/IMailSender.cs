using WebForum.Domain.Models;

namespace WebForum.Application.Abstractions.Messaging;

public interface IMailSender
{
    Task SendEmail(SimpleEmail email, CancellationToken cancellationToken = default);
}