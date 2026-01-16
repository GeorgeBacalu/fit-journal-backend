using AutoMapper;
using FitJournal.Core.Config;
using FitJournal.Core.Dtos.Common.Email;
using FitJournal.Core.Dtos.Requests.Email;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace FitJournal.Core.Services;

public class EmailService(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IEmailService
{
    public string GeneratePasswordResetEmail(PasswordResetEmail data) => $@"
        <p>Hello {data.UserName},</p>
        <p>You requested to reset your password. Click the link below:</p>
        <p><a href='{data.ResetLink}'>Reset your password</a></p>
        <p>This link expires on <strong>{data.ExpiresAt:MMMM dd, yyyy HH:mm UTC}</strong>.</p>
        <p>If you didn't request this, just ignore this email.</p>";

    public async Task SendAsync(SendEmailRequest request, CancellationToken token)
    {
        var email = new MimeMessage
        {
            Subject = request.Subject,
            Body = new TextPart(TextFormat.Html) { Text = request.Body }
        };
        email.From.Add(new MailboxAddress(AppConfig.Email.MailBoxName, AppConfig.Email.User));
        email.To.Add(MailboxAddress.Parse(request.To));

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(AppConfig.Email.SmtpHost, AppConfig.Email.SmtpPort, SecureSocketOptions.StartTls, token);
        await smtp.AuthenticateAsync(AppConfig.Email.User, AppConfig.Email.Password, token);
        await smtp.SendAsync(email, token);
        await smtp.DisconnectAsync(true, token);
    }
}
