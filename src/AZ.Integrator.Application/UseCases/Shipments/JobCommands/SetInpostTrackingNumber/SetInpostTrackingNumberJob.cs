using AZ.Integrator.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJob : JobBase<SetInpostTrackingNumberJobCommand>
{
    public SetInpostTrackingNumberJob(IMediator mediator) : base(mediator)
    {
    }
}