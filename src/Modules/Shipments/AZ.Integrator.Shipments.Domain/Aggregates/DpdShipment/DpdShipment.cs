using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Extensions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.DpdShipment;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;

public class DpdShipment : Entity, IAggregateRoot
{
    private SessionNumber _sessionNumber;
    private ExternalOrderNumber _externalOrderNumber;
    private TenantWithSourceSystemCreationInformation _creationInformation;
    private List<DpdPackage> _packages;

    public SessionNumber SessionNumber => _sessionNumber;
    public ExternalOrderNumber ExternalOrderNumber => _externalOrderNumber;
    public TenantWithSourceSystemCreationInformation CreationInformation => _creationInformation;
    public IReadOnlyCollection<DpdPackage> Packages => _packages;

    private DpdShipment()
    {
        _packages = [];
    }

    private DpdShipment(
        SessionNumber sessionNumber, IEnumerable<DpdPackage> packages,
        ExternalOrderNumber externalOrderNumber,
        TenantId tenantId, SourceSystemId sourceSystemId,
        ICurrentUser currentUser, ICurrentDateTime currentDateTime) 
        : this()
    {
        _sessionNumber = sessionNumber;
        _externalOrderNumber = externalOrderNumber;
        _creationInformation = new TenantWithSourceSystemCreationInformation(currentDateTime.CurrentDate(), currentUser.UserId, tenantId, sourceSystemId);
        _packages.AddRange(packages);
    }

    public static DpdShipment Create(
        SessionNumber sessionNumber, IEnumerable<DpdPackage> packages,
        ExternalOrderNumber externalExternalOrderNumber,
        TenantId tenantId, SourceSystemId sourceSystemId,
        ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var shipment = new DpdShipment(sessionNumber, packages, externalExternalOrderNumber, tenantId, sourceSystemId, currentUser, currentDateTime);
        
        shipment.AddDomainEvent(new DpdShipmentRegistered(
            shipment.SessionNumber, 
            shipment.ExternalOrderNumber,
            shipment.CreationInformation.SourceSystemId,
            shipment.CreationInformation.TenantId,
            shipment.CreationInformation.SourceSystemId.GetShopProviderType(),
            CorrelationIdHelper.New()));
        
        return shipment;
    }
}