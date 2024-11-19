using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;

public class InpostShipment : Entity, IAggregateRoot
{
    private ShipmentNumber _number;
    private ExternalOrderNumber _externalOrderNumber;
    private CreationInformation _creationInformation;
    private List<Parcel> _parcels;

    public ShipmentNumber Number => _number;
    public ExternalOrderNumber ExternalOrderNumber => _externalOrderNumber;
    public CreationInformation CreationInformation => _creationInformation;
    public IReadOnlyCollection<Parcel> Parcels => _parcels;

    private InpostShipment()
    {
        _parcels = [];
    }

    private InpostShipment(ShipmentNumber number, ExternalOrderNumber externalOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _number = number;
        _externalOrderNumber = externalOrderNumber;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
    }

    public static InpostShipment Create(ShipmentNumber number, ExternalOrderNumber externalExternalOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var shipment = new InpostShipment(number, externalExternalOrderNumber, currentUser, currentDateTime);
        
        shipment.AddDomainEvent(new InpostShipmentRegistered(number, externalExternalOrderNumber, currentUser.TenantId));
        
        return shipment;
    }

    public void SetTrackingNumber(List<TrackingNumber> trackingNumbers, TenantId tenantId)
    {
        trackingNumbers.ForEach(number =>
        {
            var parcel = Parcel.Create(tenantId, number);
            
            _parcels.Add(parcel);
        });
        
        AddDomainEvent(new InpostTrackingNumbersAssigned(Number, trackingNumbers.Select(x => x.Value).ToArray(), _externalOrderNumber, tenantId));
    }
}