using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ParcelTemplate;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.ViewModels;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class TagParcelTemplatesViewResolver()
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<TagParcelTemplateViewModel> GetTagParcelTemplates([Service] TagParcelTemplateDataViewContext dataViewContext) 
        => dataViewContext.TagParcelTemplates.AsQueryable();
}