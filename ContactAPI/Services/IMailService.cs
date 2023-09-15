using SendGrid.Helpers.Mail;
using SendGrid;

namespace ContactsAPI.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }

    public class MailService : IMailService
    {
        private IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendGridAPIKEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("chriswatia1@gmail.com", "Contacts API");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
