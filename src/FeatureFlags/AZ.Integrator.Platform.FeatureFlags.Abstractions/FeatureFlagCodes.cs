namespace AZ.Integrator.Platform.FeatureFlags.Abstractions;

public static class FeatureFlagCodes
{
    public static readonly string[] AllCodes =
    [
        MarketplaceModule,
        MarketplaceOrdersModule,
        MarketplaceParcelTemplatesModule,
        WarehouseModule,
        WarehouseStocksModule,
        WarehouseStatisticsModule,
        WarehouseScanningBarcodesModule,
        ProcurementModule,
        IntegrationsModule,
        InvoicesAutoGeneration,
        InvoicesAllegroSync,
        InvoicesShopifySync
    ];
    
    public const string MarketplaceModule = "marketplace-module";
    public const string MarketplaceOrdersModule = "marketplace.orders-module";
    public const string MarketplaceParcelTemplatesModule = "marketplace.parcel-templates-module";
    
    public const string WarehouseModule = "warehouse-module";
    public const string WarehouseStocksModule = "warehouse.stocks-module";
    public const string WarehouseStatisticsModule = "warehouse.statistics-module";
    public const string WarehouseScanningBarcodesModule = "warehouse.scanning-barcodes-module";
    
    public const string ProcurementModule = "procurement-module";
    
    public const string IntegrationsModule = "integrations-module";
    
    public const string InvoicesAutoGeneration = "invoices.auto-generation";
    public const string InvoicesAllegroSync = "invoices.allegro-sync";
    public const string InvoicesShopifySync = "invoices.shopify-sync";
}