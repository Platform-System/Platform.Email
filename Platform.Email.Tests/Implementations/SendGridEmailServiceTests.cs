using System.Net;
using Microsoft.Extensions.Options;
using Platform.Email.Abstractions;
using Platform.Email.Configurations;
using Platform.Email.Implementations;
using SendGrid.Helpers.Mail;
using Xunit;
using Response = SendGrid.Response;

namespace Platform.Email.Tests.Implementations;

public sealed class SendGridEmailServiceTests
{
    [Fact]
    public async Task SendEmailAsync_WhenProviderReturnsFailure_ThrowsInvalidOperationException()
    {
        var adapter = new FakeSendGridClientAdapter(
            new Response(HttpStatusCode.BadGateway, null, null));
        var service = CreateService(adapter);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.SendEmailAsync("user@example.com", "Subject", "<p>Hello</p>"));

        Assert.Contains("502", exception.Message);
    }

    [Fact]
    public async Task SendEmailAsync_WhenProviderReturnsSuccess_DoesNotThrow()
    {
        var adapter = new FakeSendGridClientAdapter(
            new Response(HttpStatusCode.Accepted, null, null));
        var service = CreateService(adapter);

        await service.SendEmailAsync("user@example.com", "Subject", "<p>Hello</p>");

        Assert.NotNull(adapter.LastMessage);
        Assert.NotEmpty(adapter.LastMessage!.Personalizations);
    }

    private static SendGridEmailService CreateService(ISendGridClientAdapter adapter)
    {
        return new SendGridEmailService(
            Options.Create(new SendGridOptions
            {
                ApiKey = "key",
                FromEmail = "noreply@example.com",
                FromName = "Platform"
            }),
            adapter);
    }

    private sealed class FakeSendGridClientAdapter : ISendGridClientAdapter
    {
        private readonly Response _response;

        public FakeSendGridClientAdapter(Response response)
        {
            _response = response;
        }

        public SendGridMessage? LastMessage { get; private set; }

        public Task<Response> SendEmailAsync(SendGridMessage message, CancellationToken cancellationToken = default)
        {
            LastMessage = message;
            return Task.FromResult(_response);
        }
    }
}
