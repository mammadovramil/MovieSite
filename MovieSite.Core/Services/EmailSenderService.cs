using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MovieSite.Core.Contracts;
using MovieSite.Core.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSite.Core.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSenderService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendEmail(string from, string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.SmtpUser, _emailSettings.SmtpPass); // Allow less secure apps: ON https://myaccount.google.com/lesssecureapps
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
