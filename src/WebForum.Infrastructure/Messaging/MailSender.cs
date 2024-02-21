using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using WebForum.Application.Abstractions.Messaging;
using WebForum.Domain.Models;
using WebForum.Infrastructure.Settings;

namespace WebForum.Infrastructure.Messaging;

public class MailSender(IOptions<MailSettings> options) : IMailSender
{
    private readonly MailSettings _mailSettings = options.Value;

    public async Task SendEmail(SimpleEmail email, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_mailSettings.Username));
        message.To.Add(MailboxAddress.Parse(email.SendTo));
        message.Subject = email.Subject;
        message.Body = new TextPart(TextFormat.Text) { Text = email.Body };

        using var smtpClient = new SmtpClient();
        // Only in development, when client antivirus software is enabled...
        smtpClient.ServerCertificateValidationCallback = (s,c,h,e) => true;
        await smtpClient.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls,
            cancellationToken);
        await smtpClient.AuthenticateAsync(_mailSettings.Username, _mailSettings.Secret, cancellationToken);
        await smtpClient.SendAsync(message, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);
    }
}