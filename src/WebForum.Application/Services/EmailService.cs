using WebForum.Application.Abstractions;
using WebForum.Domain.Entities;
using WebForum.Domain.Interfaces;
using WebForum.Domain.Models;

namespace WebForum.Application.Services;

public class EmailService(IMailSender mailSender) : IEmailService
{
    public async Task Send2FaCodeEmail(string sendTo, TwoFactorCode code, CancellationToken cancellationToken = default)
    {
        var email = new SimpleEmail()
        {
            SendTo = sendTo,
            Subject = "Your Two-Factor verification code",
            Body = $"Hi, here's your authentication code {code.Value}. This code expires in {code.Duration}"
        };
        await mailSender.SendEmail(email, cancellationToken);
    }
}