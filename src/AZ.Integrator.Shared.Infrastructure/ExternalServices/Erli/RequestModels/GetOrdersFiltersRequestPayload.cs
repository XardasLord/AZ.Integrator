using System.Text.Json.Serialization;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Erli.RequestModels;

public sealed class GetOrdersFiltersRequestPayload
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }
    
    [JsonPropertyName("filter")]
    public Filter Filter { get; set; }
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