using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.AssignTrackingNumbers;

public class AssignTrackingNumberJob : JobBase<AssignTrackingNumbersJobCommand>
{
    public AssignTrackingNumberJob(IMediator mediator) : base(mediator)
    {
    }
}