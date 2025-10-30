using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Aggregates.Supplier.Specifications;

public sealed class SupplierByNameSpec : Specification<Supplier>, ISingleResultSpecification<Supplier>
{
    public SupplierByNameSpec(string name, Guid tenantId)
    {
        Query
            .Include(x => x.Mailboxes)
            .Where(x => x.Name == name)
            .Where(x => x.CreationInformation.TenantId == new TenantId(tenantId));
    }
}