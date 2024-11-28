using System.Text.Json.Serialization;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Erli.RequestModels;

public sealed class AssignTrackingNumberRequestPayload
{
    [JsonPropertyName("deliveryTracking")]
    public DeliveryTrackingPayload DeliveryTracking { get; set; }
    
    [JsonPropertyName("externalOrderId")]
    public string ExternalOrderId { get; set; }
}

public sealed class DeliveryTrackingPayload
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("vendor")]
    public string Vendor { get; set; }
    
    [JsonPropertyName("trackingNumber")]
    public string TrackingNumber { get; set; }
}