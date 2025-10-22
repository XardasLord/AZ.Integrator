using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Catalog.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Catalog.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class CatalogViewResolver
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<FurnitureModelViewModel> GetFurnitureModels([Service] CatalogDataViewContext dataViewContext) 
        => dataViewContext.FurnitureModels.AsQueryable();
}