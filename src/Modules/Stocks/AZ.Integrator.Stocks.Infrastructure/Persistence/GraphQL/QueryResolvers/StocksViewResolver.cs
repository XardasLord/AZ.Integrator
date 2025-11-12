using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.ViewModels;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class StocksViewResolver()
{
    [RequireFeatureFlag(FeatureFlagCodes.StocksModule)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockViewModel> GetStocks(StockDataViewContext dataViewContext) 
        => dataViewContext.Stocks.AsQueryable();

    [RequireFeatureFlag(FeatureFlagCodes.StocksModule)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockGroupViewModel> GetStockGroups(StockDataViewContext dataViewContext) 
        => dataViewContext.StockGroups.AsQueryable();

    [RequireFeatureFlag(FeatureFlagCodes.StocksScanningBarcodesModule)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockLogViewModel> GetBarcodeScannerLogs(StockDataViewContext dataViewContext) 
        => dataViewContext.StockLogs.AsQueryable();
}