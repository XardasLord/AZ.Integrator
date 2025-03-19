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

    private InpostShipment(ShipmentNumber number, ExternalOrderNumber externalOrderNumber, TenantId tenantId, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _number = number;
        _externalOrderNumber = externalOrderNumber;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId, tenantId);
    }

    public static InpostShipment Create(ShipmentNumber number, ExternalOrderNumber externalOrderNumber, TenantId tenantId, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var shipment = new InpostShipment(number, externalOrderNumber, tenantId, currentUser, currentDateTime);
        
        shipment.AddDomainEvent(new InpostShipmentRegistered(number, externalOrderNumber, shipment.CreationInformation.TenantId));
        
        return shipment;
    }

    public void SetTrackingNumber(List<TrackingNumber> trackingNumbers, TenantId tenantId)
    {
        trackingNumbers.ForEach(number =>
        {
            var parcel = Parcel.Create(CreationInformation.TenantId ?? tenantId, number);
            
            _parcels.Add(parcel);
        });
        
        AddDomainEvent(new InpostTrackingNumbersAssigned(
            Number,
            trackingNumbers.Select(x => x.Value).ToArray(), 
            ExternalOrderNumber,
            CreationInformation.TenantId ?? tenantId));
    }
}