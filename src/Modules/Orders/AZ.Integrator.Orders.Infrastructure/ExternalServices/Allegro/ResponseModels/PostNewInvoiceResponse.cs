using System.Text.Json.Serialization;

namespace AZ.Integrator.Orders.Infrastructure.ExternalServices.Allegro.ResponseModels;

public sealed class PostNewInvoiceResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}