using Microsoft.Extensions.Options;
using Platform.Email.Abstractions;
using Platform.Email.Configurations;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Platform.Email.Implementations;

public class SendGridEmailService : IEmailService
{
    private readonly SendGridOptions _options;

    public SendGridEmailService(IOptions<SendGridOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlContent)
    {
        // Tạo client kết nối tới SendGrid API bằng ApiKey
        // để gửi email ra ngoài hệ thống.
        var client = new SendGridClient(_options.ApiKey);

        var from = new EmailAddress(_options.FromEmail, _options.FromName);
        var toEmail = new EmailAddress(to);

        var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, null, htmlContent);

        await client.SendEmailAsync(msg);
    }
}
