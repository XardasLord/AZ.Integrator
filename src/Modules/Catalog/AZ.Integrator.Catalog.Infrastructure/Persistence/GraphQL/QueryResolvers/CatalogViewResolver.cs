using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Catalog.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class CatalogViewResolver(ICurrentUser currentUser)
{
    [RequireFeatureFlag(FeatureFlagCodes.ProcurementModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<FurnitureModelViewModel> GetFurnitureModels(CatalogDataViewContext dataViewContext) 
        => dataViewContext.FurnitureModels
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();
}