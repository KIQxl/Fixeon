using Fixeon.Shared.Configuration;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Fixeon.Shared.Services
{
    public class EmailService : IEmailServices
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmail(EmailMessage email)
        {
            try
            {
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                mail.To.Add(MailboxAddress.Parse(email.To));
                mail.Subject = email.Subject;

                mail.Body = new TextPart("html") { Text = email.Body };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_smtpSettings.User, _smtpSettings.Password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
