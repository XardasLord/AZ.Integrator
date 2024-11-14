using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;

namespace AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;

public class TagParcel : Entity<TagParcelId>
{
    private Tag _tag;
    private TenantId _tenantId;
    private Dimension _dimension;
    private double _weight;

    public Tag Tag => _tag;
    public TenantId TenantId => _tenantId;
    public Dimension Dimension => _dimension;
    public double Weight => _weight;
    
    private TagParcel() { }

    public TagParcel(Dimension dimension, double weight)
    {
        _dimension = dimension;
        _weight = weight;
    }
}