using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;

public class InpostShipment : Entity, IAggregateRoot
{
    private ShipmentNumber _number;
    private AllegroOrderNumber _allegroAllegroOrderNumber;
    private CreationInformation _creationInformation;
    private TrackingNumber _trackingNumber;

    public ShipmentNumber Number => _number;
    public AllegroOrderNumber AllegroAllegroOrderNumber => _allegroAllegroOrderNumber;
    public CreationInformation CreationInformation => _creationInformation;
    public TrackingNumber TrackingNumber => _trackingNumber;
    
    private InpostShipment() { }

    private InpostShipment(ShipmentNumber number, AllegroOrderNumber allegroAllegroOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _number = number;
        _allegroAllegroOrderNumber = allegroAllegroOrderNumber;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
    }

    public static InpostShipment Create(ShipmentNumber number, AllegroOrderNumber allegroAllegroOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var shipment = new InpostShipment(number, allegroAllegroOrderNumber, currentUser, currentDateTime);
        
        shipment.AddDomainEvent(new InpostShipmentRegistered(number, allegroAllegroOrderNumber, currentUser.AllegroAccessToken));
        
        return shipment;
    }

    public void SetTrackingNumber(TrackingNumber trackingNumber, string allegroAccessToken)
    {
        _trackingNumber = trackingNumber;
        
        AddDomainEvent(new InpostTrackingNumberAssigned(Number, TrackingNumber, _allegroAllegroOrderNumber, allegroAccessToken));
    }
}