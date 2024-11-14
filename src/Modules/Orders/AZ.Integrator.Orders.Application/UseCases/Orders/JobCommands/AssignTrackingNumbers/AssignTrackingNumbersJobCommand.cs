using AZ.Integrator.Orders.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignTrackingNumbers;

public class AssignTrackingNumbersJobCommand : JobCommandBase
{
    public Guid OrderNumber { get; set; }
    public string[] TrackingNumbers { get; set; }
    public string TenantId { get; set; }
}