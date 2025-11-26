using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Integrations.Infrastructure.Facade;

public class IntegrationsReadFacade(IDbContextFactory<IntegrationDataViewContext> factory) : IIntegrationsReadFacade
{
    public async Task<AllegroIntegrationViewModel?> GetActiveAllegroIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Allegro
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => x.IsEnabled)
            .Where(x => !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<ErliIntegrationViewModel?> GetActiveErliIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Erli
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => x.IsEnabled)
            .Where(x => !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<ShopifyIntegrationViewModel?> GetActiveShopifyIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Shopify
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => x.IsEnabled)
            .Where(x => !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<InpostIntegrationViewModel?> GetActiveInpostIntegrationDetails(Guid tenantId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Inpost
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.IsEnabled)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<FakturowniaIntegrationViewModel?> GetActiveFakturowniaIntegrationDetails(Guid tenantId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Fakturownia
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.IsEnabled)
            .Where(x => !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<ErliIntegrationViewModel?> GetErliIntegrationBySourceSystemIdAsync(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Erli
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<ShopifyIntegrationViewModel?> GetShopifyIntegrationBySourceSystemIdAsync(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Shopify
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<FakturowniaIntegrationViewModel?> GetFakturowniaIntegrationBySourceSystemIdAsync(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Fakturownia
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<InpostIntegrationViewModel?> GetInpostIntegrationByOrganizationIdAsync(Guid tenantId, int organizationId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Inpost
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.OrganizationId == organizationId)
            .Where(x => !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }
}