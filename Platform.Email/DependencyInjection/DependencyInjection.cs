using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Platform.Email.Abstractions;
using Platform.Email.Configurations;
using Platform.Email.Constants;
using Platform.Email.Consumers;
using Platform.Email.Implementations;
using Platform.Identity.Grpc;
using Platform.Messaging.DependencyInjection;

namespace Platform.Email.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPlatformSendGridEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SendGridOptions>()
            .Bind(configuration.GetSection("SendGrid"))
            .Validate(s => !string.IsNullOrWhiteSpace(s.ApiKey), SendGridValidationMessages.ApiKeyRequired)
            .Validate(s => !string.IsNullOrWhiteSpace(s.FromEmail), SendGridValidationMessages.FromEmailRequired)
            .Validate(s => !string.IsNullOrWhiteSpace(s.FromName), SendGridValidationMessages.FromNameRequired)
            .ValidateOnStart();
        services.AddOptions<IdentityGrpcOptions>()
            .Bind(configuration.GetSection(IdentityGrpcOptions.SectionName))
            .Validate(s => !string.IsNullOrWhiteSpace(s.BaseUrl), IdentityGrpcValidationMessages.BaseUrlRequired)
            .ValidateOnStart();

        services.AddGrpcClient<IdentityIntegration.IdentityIntegrationClient>((sp, options) =>
        {
            var identityOptions = sp.GetRequiredService<IOptions<IdentityGrpcOptions>>().Value;
            options.Address = new Uri(identityOptions.BaseUrl.TrimEnd('/'));
        });
        services.AddScoped<IIdentityUserClient, IdentityUserClient>();
        services.AddScoped<IEmailService, SendGridEmailService>();

        return services;
    }

    public static IServiceCollection AddPlatformEmailProcessing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPlatformSendGridEmail(configuration);
        services.AddPlatformRabbitMqMessaging(configuration, ConfigureConsumers);

        return services;
    }

    private static void ConfigureConsumers(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<OrderInvoiceEmailRequestedConsumer>();
    }
}
