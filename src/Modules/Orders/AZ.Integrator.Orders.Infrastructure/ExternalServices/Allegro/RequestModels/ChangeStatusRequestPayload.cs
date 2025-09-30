using System.Text.Json.Serialization;
using Ardalis.SmartEnum;

namespace AZ.Integrator.Orders.Infrastructure.ExternalServices.Allegro.RequestModels;

public sealed class ChangeStatusRequestPayload
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("shipmentSummary")]
    public ShipmentSummary ShipmentSummary { get; set; }
}

public sealed class ShipmentSummary
{
    [JsonPropertyName("lineItemsSent")]
    public string LineItemsSent { get; set; }
}

public sealed class ShipmentSummaryLineItemsSentEnum : SmartEnum<ShipmentSummaryLineItemsSentEnum>
{
    public static readonly ShipmentSummaryLineItemsSentEnum None = new("NONE", 0);
    public static readonly ShipmentSummaryLineItemsSentEnum Some = new("SOME", 1);
    public static readonly ShipmentSummaryLineItemsSentEnum All = new("ALL", 2);
    
    private ShipmentSummaryLineItemsSentEnum(string name, int value) : base(name, value)
    {
    }
}