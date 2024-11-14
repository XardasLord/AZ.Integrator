using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Specifications;

public sealed class InpostShipmentByAllegroOrderNumberSpec : Specification<InpostShipment>, ISingleResultSpecification<InpostShipment>
{
    public InpostShipmentByAllegroOrderNumberSpec(AllegroOrderNumber allegroOrderNumber)
    {
        Query.Where(x => x.AllegroAllegroOrderNumber == allegroOrderNumber.Value);
    }
}