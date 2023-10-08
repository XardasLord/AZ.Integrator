using Ardalis.Specification;
using AZ.Integrator.Domain.Aggregates.InpostShipment.ValueObjects;

namespace AZ.Integrator.Domain.Aggregates.InpostShipment.Specifications;

public sealed class InpostShipmentByAllegroOrderNumberSpec : Specification<InpostShipment>, ISingleResultSpecification<InpostShipment>
{
    public InpostShipmentByAllegroOrderNumberSpec(AllegroOrderNumber allegroOrderNumber)
    {
        Query.Where(x => x.AllegroAllegroOrderNumber == allegroOrderNumber.Value);
    }
}