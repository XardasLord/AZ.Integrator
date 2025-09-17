using System.Text.Json.Serialization;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro.RequestModels;

public sealed class PostNewInvoiceRequestPayload
{
    [JsonPropertyName("invoiceNumber")]
    public string InvoiceNumber { get; set; }
    
    [JsonPropertyName("file")]
    public CheckFormsNewOrderInvoiceFile File { get; set; }
}

public sealed class CheckFormsNewOrderInvoiceFile
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}