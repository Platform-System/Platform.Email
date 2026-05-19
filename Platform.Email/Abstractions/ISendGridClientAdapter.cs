using SendGrid.Helpers.Mail;
using Response = SendGrid.Response;

namespace Platform.Email.Abstractions;

public interface ISendGridClientAdapter
{
    Task<Response> SendEmailAsync(SendGridMessage message, CancellationToken cancellationToken = default);
}
