using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;

namespace AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.Specifications;

public sealed class TagParcelTemplateByTagSpec : Specification<TagParcelTemplate>, ISingleResultSpecification<TagParcelTemplate>
{
    public TagParcelTemplateByTagSpec(Tag tag, TenantId tenantId)
    {
        Query
            .Where(x => x.Tag == tag.Value)
            .Where(x => x.TenantId == tenantId)
            .Include(x => x.Parcels);
    }
}