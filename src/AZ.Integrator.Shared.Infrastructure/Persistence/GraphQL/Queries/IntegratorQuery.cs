using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ParcelTemplate;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Stock;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;

[ExtendObjectType(Name = nameof(IntegratorQuery))]
internal class IntegratorQuery
{
    [UseProjection]
    [UseFiltering]
    public IQueryable<InpostShipmentViewModel> GetInpostShipments([Service] ShipmentDataViewContext dataViewContext) 
        => dataViewContext.InpostShipments.AsQueryable();

    [UseProjection]
    [UseFiltering]
    public IQueryable<DpdShipmentViewModel> GetDpdShipments([Service] ShipmentDataViewContext dataViewContext) 
        => dataViewContext.DpdShipments.AsQueryable();

    [UseProjection]
    [UseFiltering]
    public IQueryable<ShipmentViewModel> GetShipments([Service] ShipmentDataViewContext dataViewContext) 
        => dataViewContext.Shipments.AsQueryable();

    [UseProjection]
    [UseFiltering]
    public IQueryable<InvoiceViewModel> GetInvoices([Service] InvoiceDataViewContext dataViewContext) 
        => dataViewContext.Invoices.AsQueryable();

    [UseProjection]
    [UseFiltering]
    public IQueryable<TagParcelTemplateViewModel> GetTagParcelTemplates([Service] TagParcelTemplateDataViewContext dataViewContext) 
        => dataViewContext.TagParcelTemplates.AsQueryable();

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