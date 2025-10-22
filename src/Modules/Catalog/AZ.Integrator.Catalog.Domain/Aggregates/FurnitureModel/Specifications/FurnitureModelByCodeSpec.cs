using Ardalis.Specification;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;

public sealed class FurnitureModelByCodeSpec : Specification<FurnitureModel>, ISingleResultSpecification<FurnitureModel>
{
    public FurnitureModelByCodeSpec(string code, Guid tenantId)
    {
        Query
            .Where(x => x.FurnitureCode == code)
            .Where(x => x.CreationInformation.TenantId == new TenantId(tenantId));
    }
}