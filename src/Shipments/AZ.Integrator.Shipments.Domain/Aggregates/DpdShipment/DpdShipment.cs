using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.DpdShipment;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;

public class DpdShipment : Entity, IAggregateRoot
{
    private SessionNumber _sessionNumber;
    private AllegroOrderNumber _allegroAllegroOrderNumber;
    private CreationInformation _creationInformation;
    private List<DpdPackage> _packages;

    public SessionNumber SessionNumber => _sessionNumber;
    public AllegroOrderNumber AllegroAllegroOrderNumber => _allegroAllegroOrderNumber;
    public CreationInformation CreationInformation => _creationInformation;
    public IReadOnlyCollection<DpdPackage> Packages => _packages;

    private DpdShipment()
    {
        _packages = new List<DpdPackage>();
    }

    private DpdShipment(SessionNumber sessionNumber, IEnumerable<DpdPackage> packages, AllegroOrderNumber allegroAllegroOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime) 
        : this()
    {
        _sessionNumber = sessionNumber;
        _allegroAllegroOrderNumber = allegroAllegroOrderNumber;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
        _packages.AddRange(packages);
    }

    public static DpdShipment Create(SessionNumber sessionNumber, IEnumerable<DpdPackage> packages, AllegroOrderNumber allegroAllegroOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var shipment = new DpdShipment(sessionNumber, packages, allegroAllegroOrderNumber, currentUser, currentDateTime);
        
        shipment.AddDomainEvent(new DpdShipmentRegistered(sessionNumber, allegroAllegroOrderNumber));
        
        return shipment;
    }
}