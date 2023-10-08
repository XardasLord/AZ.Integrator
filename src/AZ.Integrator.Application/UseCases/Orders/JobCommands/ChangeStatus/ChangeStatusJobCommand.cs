using AZ.Integrator.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Application.UseCases.Orders.JobCommands.ChangeStatus;

public class ChangeStatusJobCommand : JobCommandBase
{
    public Guid OrderNumber { get; set; }
    public string AllegroAccessToken { get; set; }
}