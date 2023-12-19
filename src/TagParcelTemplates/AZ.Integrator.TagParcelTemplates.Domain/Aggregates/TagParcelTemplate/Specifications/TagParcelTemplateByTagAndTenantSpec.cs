using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;

namespace AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.Specifications;

public sealed class TagParcelTemplateByTagAndTenantSpec : Specification<TagParcelTemplate>, ISingleResultSpecification<TagParcelTemplate>
{
    public TagParcelTemplateByTagAndTenantSpec(Tag tag, TenantId tenantId)
    {
        Query
            .Where(x => x.Tag == tag.Value)
            .Where(x => x.CreationInformation.TenantId == tenantId.Value)
            .Include(x => x.Parcels);
    }
}