using AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs;

namespace AZ.Integrator.Integrations.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshAllegroTenantAccessTokenCommand : JobCommandBase
{
    public Guid TenantId { get; set; }
    public string SourceSystemId { get; set; }
}