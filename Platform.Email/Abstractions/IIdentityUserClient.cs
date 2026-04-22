using Platform.Email.Models;

namespace Platform.Email.Abstractions;

public interface IIdentityUserClient
{
    Task<IdentityUserResponse?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
