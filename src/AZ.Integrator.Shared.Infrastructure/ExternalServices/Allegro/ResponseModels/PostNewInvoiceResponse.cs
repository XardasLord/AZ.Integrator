using System.Text.Json.Serialization;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro.ResponseModels;

public sealed class PostNewInvoiceResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}