using AZ.Integrator.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJobCommand : JobCommandBase
{
    public string ShippingNumber { get; set; }
}