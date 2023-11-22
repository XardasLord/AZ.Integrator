using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;

public class DpdParcel : Entity
{
    private PackageNumber _packageId;
    private ParcelNumber _number;
    private Waybill _waybill;
    
    public ParcelNumber Number => _number;
    public Waybill Waybill => _waybill;

    private DpdParcel()
    {
    }

    private DpdParcel(ParcelNumber number, Waybill waybill) 
        : this()
    {
        _number = number;
        _waybill = waybill;
    }

    public static DpdParcel Register(ParcelNumber number, Waybill waybill)
    {
        var parcel = new DpdParcel(number, waybill);

        return parcel;
    }
}