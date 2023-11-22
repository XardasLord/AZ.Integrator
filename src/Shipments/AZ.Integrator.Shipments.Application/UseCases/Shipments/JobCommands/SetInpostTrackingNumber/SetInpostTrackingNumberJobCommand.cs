using AZ.Integrator.Shipments.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJobCommand : JobCommandBase
{
    public string ShippingNumber { get; set; }
}