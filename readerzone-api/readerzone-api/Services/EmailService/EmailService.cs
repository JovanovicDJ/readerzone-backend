using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Identity.Client;
using MimeKit;
using MimeKit.Text;
using readerzone_api.Models;

namespace readerzone_api.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendActivationEmail(string name, string address, int accountId)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = "ReaderZone - Activate your account";
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"Dear {name}, \n We are so glad that we got your request for registration " +
                       $"to ReaderZone! To activate your account click on this link: https://localhost:7297/api/activate/{accountId}" +
                       $"\nYours,\n ReaderZone team"
            };
            SendEmail(email);
        }

        public void SendForgottenPasswordEmail(string address, int accountId, long token)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = "ReaderZone - Forgotten password";
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"We have received your request for password change. In order to complete that action go " +
                       $"to this link: http://localhost:4200/login/resetPassword/{accountId}-{token} . Keep in mind" +
                       $"that this link is valid for 15 minutes since the time this email has been sent." +
                       $"\nYours,\n ReaderZone team"
            };
            SendEmail(email);
        }

        public void SendOrderReceivedEmail(string address, string name, string surname)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = "ReaderZone - Order received";
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"Dear {name} {surname}, \n We are glad to inform you that your order has been received. " +
                       $"Hard working people of ReaderZone will immediately start processing your order. " +
                       $"Soon, you will be informed when your order starts heading your way." +
                       $"\nYours,\n ReaderZone team"
            };
            SendEmail(email);
        }

        public void SendOrderProcessedEmail(string address, string name, string surname)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = "ReaderZone - Order Processed";
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"Dear {name} {surname}, \n We are glad to inform you that your order has been processed and " +
                       $"it's heading your way. \nYours,\n ReaderZone team"
            };
            SendEmail(email);
        }

        public void SendEmail(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("Email:Username").Value, _configuration.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
