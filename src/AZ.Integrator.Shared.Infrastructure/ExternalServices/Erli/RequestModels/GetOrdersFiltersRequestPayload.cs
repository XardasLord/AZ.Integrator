using System.Text.Json.Serialization;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Erli.RequestModels;

public sealed class GetOrdersFiltersRequestPayload
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }
    
    [JsonPropertyName("filter")]
    public Filter Filter { get; set; }
}

public sealed class Pagination
{
    [JsonPropertyName("sortField")]
    public string SortField { get; set; } = "created";
    
    [JsonPropertyName("order")]
    public string Order { get; set; } = "DESC";
    
    [JsonPropertyName("limit")]
    public int Limit { get; set; } = 10;
}

public sealed class Filter
{
    [JsonPropertyName("field")]
    public string Field { get; set; }
    
    [JsonPropertyName("operator")]
    public string Operator { get; set; }
    
    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public static class OrderPaginationHelper
{
    public const string CreatedAtSortField = "created";
    public const string AscOrderField = "ASC";
    public const string DescOrderField = "DESC";
    
}

public static class OrderFilterHelper
{
    public const string OrderIdField = "id";
    public const string UserEmailField = "userEmail";
    public const string CreatedAtField = "created";
    public const string UpdatedAtField = "updated";
    public const string PaymentStatusField = "paymentStatus";
}