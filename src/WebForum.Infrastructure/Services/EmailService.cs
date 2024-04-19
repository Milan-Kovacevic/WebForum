using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Models;
using WebForum.Domain.Entities;
using WebForum.Infrastructure.Options;

namespace WebForum.Infrastructure.Services;

public class EmailService(IOptions<MailOptions> options) : IEmailService
{
    private readonly MailOptions _options = options.Value;

    public Task Send2FaCodeEmail(string sendTo, TwoFactorCode code, CancellationToken cancellationToken = default)
    {
        var email = new SimpleEmail()
        {
            SendTo = sendTo,
            Subject = "Your Two-Factor verification code",
            Body =
                $"Hi, here's your authentication code {code.Value}. This code expires in {code.Duration.TotalMinutes} minutes."
        };
        return SendSimpleEmail(email, cancellationToken);
    }
    
    public async Task SendSimpleEmail(SimpleEmail email, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_options.Username));
        message.To.Add(MailboxAddress.Parse(email.SendTo));
        message.Subject = email.Subject;
        message.Body = new TextPart(TextFormat.Text) { Text = email.Body };

        using var smtpClient = new SmtpClient();
        // Only in development, when client antivirus software is enabled...
        smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
        await smtpClient.ConnectAsync(_options.SmtpHost, _options.SmtpPort, SecureSocketOptions.StartTls,
            cancellationToken);
        await smtpClient.AuthenticateAsync(_options.Username, _options.Secret, cancellationToken);
        await smtpClient.SendAsync(message, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);
    }
}