using Ardalis.Specification;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Stock.Specifications;

public sealed class StockByPackageCodeSpec : Specification<Stock>, ISingleResultSpecification<Stock>
{
    public StockByPackageCodeSpec(PackageCode code)
    {
        Query
            .Where(x => x.PackageCode == code.Value)
            .Include(x => x.StockLogs);
    }
}