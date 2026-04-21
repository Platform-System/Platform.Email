namespace Platform.Email.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlContent);
}
