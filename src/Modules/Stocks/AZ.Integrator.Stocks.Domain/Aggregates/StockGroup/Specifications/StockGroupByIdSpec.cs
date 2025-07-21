using Ardalis.Specification;

namespace AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.Specifications;

public sealed class StockGroupByIdSpec : Specification<StockGroup>, ISingleResultSpecification<StockGroup>
{
    public StockGroupByIdSpec(int groupId)
    {
        Query.Where(x => x.Id == groupId);
    }
}