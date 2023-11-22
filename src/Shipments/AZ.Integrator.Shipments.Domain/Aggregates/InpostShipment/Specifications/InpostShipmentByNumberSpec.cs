using Ardalis.Specification;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Specifications;

public sealed class InpostShipmentByNumberSpec : Specification<InpostShipment>, ISingleResultSpecification<InpostShipment>
{
    public InpostShipmentByNumberSpec(ShipmentNumber shipmentNumber)
    {
        Query.Where(x => x.Number == shipmentNumber.Value);
    }
}