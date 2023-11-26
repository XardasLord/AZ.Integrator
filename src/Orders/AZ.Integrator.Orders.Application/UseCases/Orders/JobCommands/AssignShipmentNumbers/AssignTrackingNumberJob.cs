using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignShipmentNumbers;

public class AssignTrackingNumberJob : JobBase<AssignTrackingNumberJobCommand>
{
    public AssignTrackingNumberJob(IMediator mediator) : base(mediator)
    {
    }
}