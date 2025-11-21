namespace AZ.Integrator.Platform.FeatureFlags.Infrastructure.Entities;

public class TenantFeatureFlag
{
    public Guid TenantId { get; set; }
    public string Code { get; set; } = default!;
    public bool Enabled { get; set; }
    public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid ModifiedBy { get; set; }

    public TenantFeatureFlag()
    {
    }
}