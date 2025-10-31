using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Aggregates.Supplier.Specifications;

public sealed class SupplierByIdSpec : Specification<Supplier>, ISingleResultSpecification<Supplier>
{
    public SupplierByIdSpec(int id, Guid tenantId)
    {
        Query
            .Include(x => x.Mailboxes)
            .Where(x => x.Id == id)
            .Where(x => x.CreationInformation.TenantId == new TenantId(tenantId));
    }
}