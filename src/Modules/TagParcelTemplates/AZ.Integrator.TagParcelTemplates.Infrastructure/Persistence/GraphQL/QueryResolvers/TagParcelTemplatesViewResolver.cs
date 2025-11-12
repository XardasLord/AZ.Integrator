using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.ViewModels;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class TagParcelTemplatesViewResolver
{
    [RequireFeatureFlag(FeatureFlagCodes.ParcelTemplatesModule)]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<TagParcelTemplateViewModel> GetTagParcelTemplates(TagParcelTemplateDataViewContext dataViewContext) 
        => dataViewContext.TagParcelTemplates.AsQueryable();
}