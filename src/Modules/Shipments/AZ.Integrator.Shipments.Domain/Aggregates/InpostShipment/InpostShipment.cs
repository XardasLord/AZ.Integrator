using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Extensions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;

public class InpostShipment : Entity, IAggregateRoot
{
    private ShipmentNumber _number;
    private ExternalOrderNumber _externalOrderNumber;
    private TenantWithSourceSystemCreationInformation _creationInformation;
    private List<Parcel> _parcels;

    public ShipmentNumber Number => _number;
    public ExternalOrderNumber ExternalOrderNumber => _externalOrderNumber;
    public TenantWithSourceSystemCreationInformation CreationInformation => _creationInformation;
    public IReadOnlyCollection<Parcel> Parcels => _parcels;

    private InpostShipment()
    {
        _parcels = [];
    }

    private InpostShipment(
        ShipmentNumber number, ExternalOrderNumber externalOrderNumber, 
        TenantId tenantId, SourceSystemId sourceSystemId, 
        ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _number = number;
        _externalOrderNumber = externalOrderNumber;
        _creationInformation = new TenantWithSourceSystemCreationInformation(currentDateTime.CurrentDate(), currentUser.UserId, tenantId, sourceSystemId);
    }

    public static InpostShipment Create(
        ShipmentNumber number, ExternalOrderNumber externalOrderNumber,
        TenantId tenantId, SourceSystemId sourceSystemId,
        ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var shipment = new InpostShipment(number, externalOrderNumber, tenantId, sourceSystemId, currentUser, currentDateTime);
            
        shipment.AddDomainEvent(new InpostShipmentRegistered(
            shipment.Number,
            shipment.ExternalOrderNumber,
            shipment.CreationInformation.SourceSystemId,
            shipment.CreationInformation.TenantId,
            shipment.CreationInformation.SourceSystemId.GetShopProviderType(),
            CorrelationIdHelper.New()));
        
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
            CreationInformation.TenantId ?? tenantId,
            CreationInformation.SourceSystemId));
    }
}