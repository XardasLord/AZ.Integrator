using AZ.Integrator.Domain.SharedKernel;

namespace AZ.Integrator.Shared.Application;

public record HeaderRequest
{
    public string TenantId { get; set; }
    public string SourceSystemId { get; set; }
    public ShopProviderType ShopProvider { get; set; }
}