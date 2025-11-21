using AZ.Integrator.Integrations.Contracts.ViewModels;

namespace AZ.Integrator.Integrations.Contracts;

public interface IIntegrationsReadFacade
{
    Task<AllegroIntegrationViewModel?> GetAllegroIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<ErliIntegrationViewModel?> GetErliIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<ShopifyIntegrationViewModel?> GetShopifyIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<InpostIntegrationViewModel?> GetInpostIntegrationDetails(Guid tenantId, CancellationToken cancellationToken = default);
    Task<FakturowniaIntegrationViewModel?> GetFakturowniaIntegrationDetails(Guid tenantId, CancellationToken cancellationToken = default);
}