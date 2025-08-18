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
    private TenantCreationInformation _creationInformation;
    private List<DpdPackage> _packages;

    public SessionNumber SessionNumber => _sessionNumber;
    public ExternalOrderNumber ExternalOrderNumber => _externalOrderNumber;
    public TenantCreationInformation CreationInformation => _creationInformation;
    public IReadOnlyCollection<DpdPackage> Packages => _packages;

    private DpdShipment()
    {
        _packages = [];
    }

    private DpdShipment(SessionNumber sessionNumber, IEnumerable<DpdPackage> packages, ExternalOrderNumber externalOrderNumber, TenantId tenantId, ICurrentUser currentUser, ICurrentDateTime currentDateTime) 
        : this()
    {
        _sessionNumber = sessionNumber;
        _externalOrderNumber = externalOrderNumber;
        _creationInformation = new TenantCreationInformation(currentDateTime.CurrentDate(), currentUser.UserId, tenantId);
        _packages.AddRange(packages);
    }

    public static DpdShipment Create(SessionNumber sessionNumber, IEnumerable<DpdPackage> packages, ExternalOrderNumber externalExternalOrderNumber, TenantId tenantId, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var shipment = new DpdShipment(sessionNumber, packages, externalExternalOrderNumber, tenantId, currentUser, currentDateTime);
        
        shipment.AddDomainEvent(new DpdShipmentRegistered(sessionNumber, externalExternalOrderNumber));
        
        return shipment;
    }
}