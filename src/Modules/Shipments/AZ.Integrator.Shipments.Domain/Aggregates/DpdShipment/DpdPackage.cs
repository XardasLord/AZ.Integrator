using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;

public class DpdPackage : Entity
{
    private SessionNumber _shipmentId;
    private PackageNumber _number;
    private List<DpdParcel> _parcels;

    public PackageNumber Number => _number;
    public IReadOnlyCollection<DpdParcel> Parcels => _parcels;

    private DpdPackage()
    {
        _parcels = new List<DpdParcel>();
    }
    
    private DpdPackage(PackageNumber number, IEnumerable<DpdParcel> parcels) 
        : this()
    {
        _number = number;
        _parcels.AddRange(parcels);
    }

    public static DpdPackage Register(PackageNumber number, IEnumerable<DpdParcel> parcels)
    {
        var package = new DpdPackage(number, parcels);

        return package;
    }
}