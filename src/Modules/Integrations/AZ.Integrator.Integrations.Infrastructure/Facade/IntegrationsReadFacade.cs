using AZ.Integrator.Integrations.Contracts;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Integrations.Infrastructure.Facade;

public class IntegrationsReadFacade(IDbContextFactory<IntegrationDataViewContext> factory) : IIntegrationsReadFacade
{
    public async Task<AllegroIntegrationViewModel?> GetAllegroIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Allegro
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => x.IsEnabled)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<ErliIntegrationViewModel?> GetErliIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Erli
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => x.IsEnabled)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<ShopifyIntegrationViewModel?> GetShopifyIntegrationDetails(Guid tenantId, string sourceSystemId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Shopify
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.SourceSystemId == sourceSystemId)
            .Where(x => x.IsEnabled)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<InpostIntegrationViewModel?> GetInpostIntegrationDetails(Guid tenantId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Inpost
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.IsEnabled)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }

    public async Task<FakturowniaIntegrationViewModel?> GetFakturowniaIntegrationDetails(Guid tenantId, CancellationToken cancellationToken = default)
    {
        await using var dataViewContext = await factory.CreateDbContextAsync(cancellationToken);
        
        var details = await dataViewContext.Fakturownia
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.IsEnabled)
            .SingleOrDefaultAsync(cancellationToken);

        return details;
    }
}