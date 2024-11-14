using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeAllegroOrderStatus;

public class ChangeAllegroOrderStatusJob : JobBase<ChangeAllegroOrderStatusJobCommand>
{
    public ChangeAllegroOrderStatusJob(IMediator mediator) : base(mediator)
    {
    }
}