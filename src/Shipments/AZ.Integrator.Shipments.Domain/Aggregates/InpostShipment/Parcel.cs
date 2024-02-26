using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;

public class Parcel : Entity
{
    private ShipmentNumber _shipmentNumber;
    private TenantId _tenantId;
    private TrackingNumber _trackingNumber;

    public TenantId TenantId => _tenantId;
    public TrackingNumber TrackingNumber => _trackingNumber;
    
    private Parcel() { }

    private Parcel(TenantId tenantId, TrackingNumber trackingNumber)
    {
        _tenantId = tenantId;
        _trackingNumber = trackingNumber;
    }

    public static Parcel Create(TenantId tenantId, TrackingNumber trackingNumber) 
        => new(tenantId, trackingNumber);
}