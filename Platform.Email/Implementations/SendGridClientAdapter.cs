using Platform.Email.Abstractions;
using Platform.Email.Configurations;
using SendGrid;
using SendGrid.Helpers.Mail;
using Response = SendGrid.Response;

namespace Platform.Email.Implementations;

public sealed class SendGridClientAdapter : ISendGridClientAdapter
{
    private readonly SendGridOptions _options;

    public SendGridClientAdapter(SendGridOptions options)
    {
        _options = options;
    }

    public Task<Response> SendEmailAsync(SendGridMessage message, CancellationToken cancellationToken = default)
    {
        var client = new SendGridClient(_options.ApiKey);
        return client.SendEmailAsync(message, cancellationToken);
    }
}
