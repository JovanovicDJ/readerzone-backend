using MimeKit;

namespace readerzone_api.Services.EmailService
{
    public interface IEmailService
    {
        public void SendEmail(MimeMessage email);
        public void SendActivationEmail(string name, string email, int accountId);
        public void SendForgottenPasswordEmail(string address, int accountId, long token);
        public void SendOrderReceivedEmail(string address, string name, string surname);
        public void SendOrderProcessedEmail(string address, string name, string surname);

    }
}
