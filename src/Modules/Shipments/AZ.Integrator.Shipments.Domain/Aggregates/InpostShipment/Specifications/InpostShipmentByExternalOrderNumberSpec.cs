using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Specifications;

public sealed class InpostShipmentByExternalOrderNumberSpec : Specification<InpostShipment>, ISingleResultSpecification<InpostShipment>
{
    public InpostShipmentByExternalOrderNumberSpec(ExternalOrderNumber externalOrderNumber, TenantId tenantId)
    {
        Query
            .Where(x => x.ExternalOrderNumber == externalOrderNumber.Value)
            .Where(x => x.CreationInformation.TenantId == tenantId);
    }
}