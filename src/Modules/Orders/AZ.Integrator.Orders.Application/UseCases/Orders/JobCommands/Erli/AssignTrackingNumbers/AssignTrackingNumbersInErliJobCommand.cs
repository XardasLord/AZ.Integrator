using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Erli.AssignTrackingNumbers;

public class AssignTrackingNumbersInErliJobCommand : JobCommandBase
{
    public string OrderNumber { get; set; }
    public string[] TrackingNumbers { get; set; }
    public string Vendor { get; set; }
    public string DeliveryTrackingStatus { get; set; }
    public string TenantId { get; set; }
}