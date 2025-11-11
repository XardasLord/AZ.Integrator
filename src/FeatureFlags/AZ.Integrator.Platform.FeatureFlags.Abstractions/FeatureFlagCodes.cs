namespace AZ.Integrator.Platform.FeatureFlags.Abstractions;

public static class FeatureFlagCodes
{
    public const string OrdersModule = "orders-module";
    public const string ParcelTemplatesModule = "parcel-templates-module";
    public const string StocksModule = "stocks-module";
    public const string ProcurementModule = "procurement-module";
    
    public const string StocksStatisticsEnabled = "stocks.statistics-enabled";
    public const string StocksScanningBarcodes = "stocks.scanning-barcodes";
    
    public const string InvoicesAutoGeneration = "invoices.auto-generation";
    public const string InvoicesAllegroSync = "invoices.allegro-sync";
    public const string InvoicesShopifySync = "invoices.shopify-sync";
}