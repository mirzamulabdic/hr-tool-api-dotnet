using API.DTOs;
using API.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Asn1.Ocsp;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfigurationDto _emailConfig;

        public EmailService(EmailConfigurationDto emailConfig)
        {
           _emailConfig = emailConfig;
        }
        public void SendEmail(EmailMessageDto emailMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailConfig.From));
            email.To.Add(MailboxAddress.Parse(emailMessage.To));
            email.Subject = emailMessage.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailMessage.Content };

            using var smtp = new SmtpClient();
            smtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailConfig.From, _emailConfig.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
