using MassTransit;
using Platform.Contracts.Messages.Emails;
using Platform.Email.Abstractions;
using Platform.Email.Formatters;

namespace Platform.Email.Consumers;

public sealed class OrderInvoiceEmailRequestedConsumer : IConsumer<OrderInvoiceEmailRequested>
{
    private readonly IEmailService _emailService;
    private readonly IIdentityUserClient _identityUserClient;

    public OrderInvoiceEmailRequestedConsumer(IEmailService emailService, IIdentityUserClient identityUserClient)
    {
        _emailService = emailService;
        _identityUserClient = identityUserClient;
    }

    public async Task Consume(ConsumeContext<OrderInvoiceEmailRequested> context)
    {
        var user = await _identityUserClient.GetUserAsync(context.Message.UserId, context.CancellationToken);
        if (user is null || string.IsNullOrWhiteSpace(user.Email))
            return;

        var subject = $"Invoice for order #{context.Message.OrderCode}";
        var htmlContent = InvoiceEmailFormatter.Format(user.UserName, context.Message);

        await _emailService.SendEmailAsync(user.Email, subject, htmlContent);
    }
}
