using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Specifications;

public sealed class DpdShipmentByExternalOrderNumberSpec : Specification<DpdShipment>, ISingleResultSpecification<DpdShipment>
{
    public DpdShipmentByExternalOrderNumberSpec(ExternalOrderNumber externalOrderNumber, TenantId tenantId)
    {
        Query
            .Where(x => x.ExternalOrderNumber == externalOrderNumber.Value)
            .Where(x => x.CreationInformation.TenantId == tenantId);
    }
}