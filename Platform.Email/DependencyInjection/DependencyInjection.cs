using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Email.Abstractions;
using Platform.Email.Configurations;
using Platform.Email.Constants;
using Platform.Email.Implementations;

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

        services.AddScoped<IEmailService, SendGridEmailService>();

        return services;
    }
}
