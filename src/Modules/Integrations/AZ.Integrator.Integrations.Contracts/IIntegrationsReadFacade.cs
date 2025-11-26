using AZ.Integrator.Integrations.Contracts.ViewModels;

namespace AZ.Integrator.Integrations.Contracts;

public interface IIntegrationsReadFacade
{
    Task<AllegroIntegrationViewModel?> GetActiveAllegroIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<ErliIntegrationViewModel?> GetActiveErliIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<ShopifyIntegrationViewModel?> GetActiveShopifyIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<InpostIntegrationViewModel?> GetActiveInpostIntegrationDetails(Guid tenantId, CancellationToken cancellationToken = default);
    Task<FakturowniaIntegrationViewModel?> GetActiveFakturowniaIntegrationDetails(Guid tenantId, CancellationToken cancellationToken = default);

    Task<ErliIntegrationViewModel?> GetErliIntegrationBySourceSystemIdAsync(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<ShopifyIntegrationViewModel?> GetShopifyIntegrationBySourceSystemIdAsync(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<FakturowniaIntegrationViewModel?> GetFakturowniaIntegrationBySourceSystemIdAsync(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default);
    Task<InpostIntegrationViewModel?> GetInpostIntegrationByOrganizationIdAsync(Guid tenantId, int organizationId, CancellationToken cancellationToken = default);
}