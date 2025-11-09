namespace AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshTenantAccessTokenCommand : JobCommandBase
{
    public string TenantId { get; set; }
    public string SourceSystemId { get; set; }
}