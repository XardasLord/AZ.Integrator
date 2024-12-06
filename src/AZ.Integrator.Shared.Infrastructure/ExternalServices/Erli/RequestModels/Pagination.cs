using System.Text.Json.Serialization;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Erli.RequestModels;

public sealed class Pagination
{
    [JsonPropertyName("sortField")]
    public string SortField { get; set; } = "created";
    
    [JsonPropertyName("order")]
    public string Order { get; set; } = "DESC";
    
    [JsonPropertyName("limit")]
    public int Limit { get; set; } = 10;
}