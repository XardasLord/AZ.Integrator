using System.Text.Json.Serialization;

namespace AZ.Integrator.Orders.Infrastructure.ExternalServices.Erli.RequestModels;

public sealed class Filter
{
    [JsonPropertyName("field")]
    public string Field { get; set; }
    
    [JsonPropertyName("operator")]
    public string Operator { get; set; }
    
    [JsonPropertyName("value")]
    public string Value { get; set; }
}