using Platform.Email.Abstractions;
using Platform.Email.Models;
using Platform.Common.Grpc;
using Platform.Identity.Grpc;

namespace Platform.Email.Implementations;

public sealed class IdentityUserClient : IIdentityUserClient
{
    private readonly IdentityIntegration.IdentityIntegrationClient _client;

    public IdentityUserClient(IdentityIntegration.IdentityIntegrationClient client)
    {
        _client = client;
    }

    public async Task<IdentityUserResponse?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetUserEmailProfileAsync(
            new GetUserEmailProfileRequest
            {
                UserId = userId.ToString()
            },
            cancellationToken: cancellationToken);

        if (response.Status.IsFailure() || response.Data is null)
            return null;

        return new IdentityUserResponse
        {
            Id = Guid.Parse(response.Data.Id),
            IdentityId = Guid.Parse(response.Data.IdentityId),
            UserName = response.Data.UserName,
            Email = response.Data.Email
        };
    }
}
