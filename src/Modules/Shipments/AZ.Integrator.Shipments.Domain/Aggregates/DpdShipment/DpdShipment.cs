using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.DpdShipment;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;

public class DpdShipment : Entity, IAggregateRoot
{
    private SessionNumber _sessionNumber;
    private ExternalOrderNumber _externalOrderNumber;
    private CreationInformation _creationInformation;
    private List<DpdPackage> _packages;

    public SessionNumber SessionNumber => _sessionNumber;
    public ExternalOrderNumber ExternalOrderNumber => _externalOrderNumber;
    public CreationInformation CreationInformation => _creationInformation;
    public IReadOnlyCollection<DpdPackage> Packages => _packages;

    private DpdShipment()
    {
        _packages = new List<DpdPackage>();
    }

    private DpdShipment(SessionNumber sessionNumber, IEnumerable<DpdPackage> packages, ExternalOrderNumber externalOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime) 
        : this()
    {
        _sessionNumber = sessionNumber;
        _externalOrderNumber = externalOrderNumber;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
        _packages.AddRange(packages);
    }

    public static DpdShipment Create(SessionNumber sessionNumber, IEnumerable<DpdPackage> packages, ExternalOrderNumber externalExternalOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var shipment = new DpdShipment(sessionNumber, packages, externalExternalOrderNumber, currentUser, currentDateTime);
        
        shipment.AddDomainEvent(new DpdShipmentRegistered(sessionNumber, externalExternalOrderNumber));
        
        return shipment;
    }
}