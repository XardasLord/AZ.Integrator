using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.Specifications;

public sealed class OrderByNumberSpec : Specification<PartDefinitionsOrder>, ISingleResultSpecification<PartDefinitionsOrder>
{
    public OrderByNumberSpec(string number, Guid tenantId)
    {
        Query
            .Where(x => x.Number == number)
            .Where(x => x.CreationInformation.TenantId == new TenantId(tenantId));
    }
}