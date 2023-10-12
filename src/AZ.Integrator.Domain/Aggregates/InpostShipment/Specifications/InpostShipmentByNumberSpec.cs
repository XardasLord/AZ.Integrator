using Ardalis.Specification;
using AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment.Specifications;

public sealed class InpostShipmentByNumberSpec : Specification<InpostShipment>, ISingleResultSpecification<InpostShipment>
{
    public InpostShipmentByNumberSpec(ShipmentNumber shipmentNumber)
    {
        Query.Where(x => x.Number == shipmentNumber.Value);
    }
}