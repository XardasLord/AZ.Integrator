using AZ.Integrator.Orders.Application.Common.BackgroundJobs;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.ChangeOrderStatus;

public class ChangeAllegroOrderStatusJobCommand : JobCommandBase
{
    public Guid OrderNumber { get; set; }
    public int OrderStatus { get; set; }
    public Guid TenantId { get; set; }
    public string SourceSystemId { get; set; }
}