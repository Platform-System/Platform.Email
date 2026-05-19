using Microsoft.Extensions.Options;
using Platform.Email.Abstractions;
using Platform.Email.Configurations;
using SendGrid.Helpers.Mail;

namespace Platform.Email.Implementations;

public class SendGridEmailService : IEmailService
{
    private readonly SendGridOptions _options;
    private readonly ISendGridClientAdapter _sendGridClient;

    public SendGridEmailService(IOptions<SendGridOptions> options, ISendGridClientAdapter sendGridClient)
    {
        _options = options.Value;
        _sendGridClient = sendGridClient;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlContent)
    {
        var from = new EmailAddress(_options.FromEmail, _options.FromName);
        var toEmail = new EmailAddress(to);

        var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, null, htmlContent);

        var response = await _sendGridClient.SendEmailAsync(msg);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"SendGrid email delivery failed with status code {(int)response.StatusCode}.");
        }
    }
}
