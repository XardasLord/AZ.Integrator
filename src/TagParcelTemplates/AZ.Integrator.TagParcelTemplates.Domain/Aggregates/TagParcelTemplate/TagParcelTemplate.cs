using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;

namespace AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;

public class TagParcelTemplate : Entity, IAggregateRoot
{
    private Tag _tag;
    private CreationInformationWithTenant _creationInformation;
    private List<TagParcel> _parcels;

    public Tag Tag => _tag;
    public CreationInformationWithTenant CreationInformation => _creationInformation;
    public IReadOnlyCollection<TagParcel> Parcels => _parcels;

    private TagParcelTemplate()
    {
        _parcels = new List<TagParcel>();
    }

    private TagParcelTemplate(Tag tag, IEnumerable<TagParcel> parcels, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _tag = tag;
        _creationInformation = new CreationInformationWithTenant(currentDateTime.CurrentDate(), currentUser.UserId, currentUser.TenantId);
        _parcels = parcels.ToList();
    }

    public static TagParcelTemplate Create(Tag tag, IEnumerable<TagParcel> parcels, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var template = new TagParcelTemplate(tag, parcels, currentUser, currentDateTime);
        
        // template.AddDomainEvent(new InpostShipmentRegistered(number, allegroAllegroOrderNumber, currentUser.TenantId));
        
        return template;
    }

    public void SetParcels(IEnumerable<TagParcel> parcels)
    {
        _parcels = parcels.ToList();
    }
}