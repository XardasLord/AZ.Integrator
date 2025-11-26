using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class IntegrationViewResolver(ICurrentUser currentUser)
{
    [RequireFeatureFlag(FeatureFlagCodes.IntegrationsModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AllegroIntegrationViewModel> GetAllegroIntegrations(IntegrationDataViewContext dataViewContext) 
        => dataViewContext.Allegro
            .Where(x => x.TenantId == currentUser.TenantId)
            .Where(x => !x.IsDeleted)
            .AsQueryable();
    
    [RequireFeatureFlag(FeatureFlagCodes.IntegrationsModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ErliIntegrationViewModel> GetErliIntegrations(IntegrationDataViewContext dataViewContext) 
        => dataViewContext.Erli
            .Where(x => x.TenantId == currentUser.TenantId)
            .Where(x => !x.IsDeleted)
            .AsQueryable();
    
    [RequireFeatureFlag(FeatureFlagCodes.IntegrationsModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ShopifyIntegrationViewModel> GetShopifyIntegrations(IntegrationDataViewContext dataViewContext) 
        => dataViewContext.Shopify
            .Where(x => x.TenantId == currentUser.TenantId)
            .Where(x => !x.IsDeleted)
            .AsQueryable();
    
    [RequireFeatureFlag(FeatureFlagCodes.IntegrationsModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<InpostIntegrationViewModel> GetInpostIntegrations(IntegrationDataViewContext dataViewContext) 
        => dataViewContext.Inpost
            .Where(x => x.TenantId == currentUser.TenantId)
            .Where(x => !x.IsDeleted)
            .AsQueryable();
    
    [RequireFeatureFlag(FeatureFlagCodes.IntegrationsModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<FakturowniaIntegrationViewModel> GetFakturowniaIntegrations(IntegrationDataViewContext dataViewContext) 
        => dataViewContext.Fakturownia
            .Where(x => x.TenantId == currentUser.TenantId)
            .Where(x => !x.IsDeleted)
            .AsQueryable();
}