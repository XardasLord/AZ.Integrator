using Ardalis.Specification;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;

namespace AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.Specifications;

public sealed class TagParcelTemplateByTagSpec : Specification<TagParcelTemplate>, ISingleResultSpecification<TagParcelTemplate>
{
    public TagParcelTemplateByTagSpec(Tag tag)
    {
        Query
            .Where(x => x.Tag == tag.Value)
            .Include(x => x.Parcels);
    }
}