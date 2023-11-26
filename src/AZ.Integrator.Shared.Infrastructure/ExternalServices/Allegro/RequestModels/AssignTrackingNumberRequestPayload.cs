using System.Text.Json.Serialization;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro.RequestModels;

public sealed class AssignTrackingNumberRequestPayload
{
    [JsonPropertyName("carrierId")]
    public string CarrierId { get; set; }
    
    [JsonPropertyName("waybill")]
    public string TrackingNumber { get; set; }
}