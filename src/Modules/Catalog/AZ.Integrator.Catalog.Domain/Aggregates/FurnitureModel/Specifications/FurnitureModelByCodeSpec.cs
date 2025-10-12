using Ardalis.Specification;

namespace AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.Specifications;

public sealed class FurnitureModelByCodeSpec : Specification<FurnitureModel>, ISingleResultSpecification<FurnitureModel>
{
    public FurnitureModelByCodeSpec(string code, Guid tenantId)
    {
        Query
            .Where(x => x.FurnitureCode.Value == code)
            .Where(x => x.CreationInformation.TenantId.Value == tenantId);
    }
}