using System.Net;
using Microsoft.Extensions.Options;
using Server.Application.Common.Interfaces.Services;
using Server.Contracts.Common;
using System.Net.Mail;

namespace Server.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            emailSettings = options.Value;
        }
        public void SendEmail(MailRequest mailRequest)
        {
            string fromMail = emailSettings.Email;
            string fromPassword = emailSettings.Password;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = mailRequest.Subject;
            message.To.Add(new MailAddress(mailRequest.ToEmail));
            message.Body = mailRequest.Body;
            message.IsBodyHtml = false;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };
                smtpClient.Send(message);

        }
    }
}
