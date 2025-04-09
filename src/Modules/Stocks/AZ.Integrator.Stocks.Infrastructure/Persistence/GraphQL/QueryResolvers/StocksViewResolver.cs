using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class StocksViewResolver()
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockViewModel> GetStocks([Service] StockDataViewContext dataViewContext) 
        => dataViewContext.Stocks.AsQueryable();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockLogViewModel> GetBarcodeScannerLogs([Service] StockDataViewContext dataViewContext) 
        => dataViewContext.StockLogs.AsQueryable();
}