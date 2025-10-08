using AZ.Integrator.Orders.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignTrackingNumbers;

public class AssignTrackingNumbersInAllegroJobCommand : JobCommandBase
{
    public Guid OrderNumber { get; set; }
    public string[] TrackingNumbers { get; set; }
    public Guid TenantId { get; set; }
    public string SourceSystemId { get; set; }
}