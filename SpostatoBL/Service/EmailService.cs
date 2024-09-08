using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace SpostatoBL.Service
{
    public class EmailService: IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _username;
        private readonly string _appPassword; 

        public EmailService(string smtpServer, int port, string username, string appPassword)
        {
            _smtpServer = smtpServer;
            _port = port;
            _username = username;
            _appPassword = appPassword;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_username));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpServer, _port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_username, _appPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }

    // Usage example:
    // var emailService = new EmailService("smtp.gmail.com", 587, "your-email@gmail.com", "your-app-specific-password");
    // await emailService.SendEmailAsync("recipient@example.com", "Test Subject", "This is a test email.");

    // Note: To generate an app-specific password:
    // 1. Go to your Google Account settings
    // 2. Navigate to Security
    // 3. Under "Signing in to Google", select "App passwords"
    // 4. Generate a new app password for "Mail" and your specific app/device
    // 5. Use this generated password in place of your regular password
}
