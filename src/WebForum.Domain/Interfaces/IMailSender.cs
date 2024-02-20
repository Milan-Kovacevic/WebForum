using WebForum.Domain.Models;

namespace WebForum.Domain.Interfaces;

public interface IMailSender
{
    Task SendEmail(SimpleEmail email, CancellationToken cancellationToken = default);
}