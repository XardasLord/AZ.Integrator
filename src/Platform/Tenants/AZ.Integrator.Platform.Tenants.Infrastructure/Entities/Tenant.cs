using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Entities;

namespace AZ.Integrator.Platform.Tenants.Infrastructure.Entities;

public class Tenant
{
    public Guid TenantId { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public ICollection<TenantFeatureFlag> FeatureFlags;

    public Tenant()
    {
        FeatureFlags = [];
    }
}