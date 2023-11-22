using AZ.Integrator.Shipments.Application.Common.BackgroundJobs;
using MediatR;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJob : JobBase<SetInpostTrackingNumberJobCommand>
{
    public SetInpostTrackingNumberJob(IMediator mediator) : base(mediator)
    {
    }
}