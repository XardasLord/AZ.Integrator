using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeAllegroOrderStatus;

public class ChangeAllegroOrderStatusJobCommand : JobCommandBase
{
    public Guid OrderNumber { get; set; }
    public AllegroFulfillmentStatusEnum OrderStatus { get; set; }
    public string AllegroAccessToken { get; set; }
}