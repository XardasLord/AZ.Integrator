using System.Text.Json.Serialization;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Erli.RequestModels;

public sealed class GetProductsFiltersRequestPayload
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }
    
    [JsonPropertyName("filter")]
    public Filter Filter { get; set; }
    
    [JsonPropertyName("fields")]
    public string[] Fields { get; set; }
}

public static class ProductPaginationHelper
{
    public const string ModifiedAtSortField = "updated";
    public const string Asc = "ASC";
    public const string Desc = "DESC";
    
}

public static class ProductFilterHelper
{
    public const string Status = "status";
    public const string Sku = "sku";
}

public static class ProductFieldsHelper
{
    public const string Name = "name";
    public const string Sku = "sku";
    public const string Status = "status";
    public const string ExternalId = "externalId";
}