namespace Platform.Email.Models;

public sealed class IdentityUserResponse
{
    public Guid Id { get; set; }
    public Guid IdentityId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
