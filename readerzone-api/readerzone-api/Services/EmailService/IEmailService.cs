using MimeKit;

namespace readerzone_api.Services.EmailService
{
    public interface IEmailService
    {
        public void SendEmail(MimeMessage email);
        public void SendActivationEmail(string name, string email, int accountId);
    }
}
