using AZ.Integrator.Orders.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignShipmentNumbers;

public class ChangeStatusJobCommand : JobCommandBase
{
    public Guid OrderNumber { get; set; }
    public string AllegroAccessToken { get; set; }
}