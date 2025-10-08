using AZ.Integrator.Domain.SharedKernel;

namespace AZ.Integrator.Shared.Application;

public record HeaderRequest
{
    public Guid TenantId { get; set; }
    public string SourceSystemId { get; set; }
    public ShopProviderType ShopProvider { get; set; }
}