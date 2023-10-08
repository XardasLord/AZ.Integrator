using AZ.Integrator.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.JobCommands.ChangeStatus;

public class ChangeStatusJob : JobBase<ChangeStatusJobCommand>
{
    public ChangeStatusJob(IMediator mediator) : base(mediator)
    {
    }
}