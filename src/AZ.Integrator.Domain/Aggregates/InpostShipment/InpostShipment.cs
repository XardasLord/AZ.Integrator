using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.InpostShipment.DomainEvents;
using AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment;

public class InpostShipment : Entity, IAggregateRoot
{
    private ShipmentNumber _number;
    private AllegroOrderNumber _allegroAllegroOrderNumber;
    private CreationInformation _creationInformation;

    public ShipmentNumber Number => _number;
    public AllegroOrderNumber AllegroAllegroOrderNumber => _allegroAllegroOrderNumber;
    public CreationInformation CreationInformation => _creationInformation;
    
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
        
        shipment.AddDomainEvent(new InpostShipmentRegistered(shipment.Number));
        
        return shipment;
    }
}