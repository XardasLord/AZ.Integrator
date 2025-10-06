using AZ.Integrator.Shipments.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJobCommand : JobCommandBase
{
    public string ShippingNumber { get; set; }
    public string ExternalOrderNumber { get; set; }

    public string SourceSystemId { get; set; }

    [Obsolete("In the future we will be able to remove this parameter, because already the Shipment itself tracks this information about TenantId")]
    public string TenantId { get; set; }
    public string CorrelationId { get; set; }
}