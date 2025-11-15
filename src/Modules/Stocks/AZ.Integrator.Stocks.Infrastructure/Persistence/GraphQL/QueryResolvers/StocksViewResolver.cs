using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View;
using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.ViewModels;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.GraphQL.QueryResolvers;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
public class StocksViewResolver(ICurrentUser currentUser)
{
    [RequireFeatureFlag(FeatureFlagCodes.WarehouseStocksModule)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockViewModel> GetStocks(StockDataViewContext dataViewContext) 
        => dataViewContext.Stocks
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();

    [RequireFeatureFlag(FeatureFlagCodes.WarehouseStocksModule)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockGroupViewModel> GetStockGroups(StockDataViewContext dataViewContext)
        => dataViewContext.StockGroups
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();

    [RequireFeatureFlag(FeatureFlagCodes.WarehouseScanningBarcodesModule)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockLogViewModel> GetBarcodeScannerLogs(StockDataViewContext dataViewContext)
        => dataViewContext.StockLogs
            .Where(x => x.TenantId == currentUser.TenantId)
            .AsQueryable();
}