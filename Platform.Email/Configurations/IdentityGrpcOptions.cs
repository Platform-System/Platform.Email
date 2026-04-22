namespace Platform.Email.Configurations;

public sealed class IdentityGrpcOptions
{
    public const string SectionName = "Integrations:Identity";

    public string BaseUrl { get; set; } = string.Empty;
}
