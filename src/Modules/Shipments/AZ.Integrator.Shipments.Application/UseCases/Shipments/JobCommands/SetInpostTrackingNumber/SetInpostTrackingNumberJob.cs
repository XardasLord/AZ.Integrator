using AZ.Integrator.Shipments.Application.Common.BackgroundJobs;
using Mediator;

namespace AZ.Integrator.Shipments.Application.UseCases.Shipments.JobCommands.SetInpostTrackingNumber;

public class SetInpostTrackingNumberJob(IMediator mediator) : JobBase<SetInpostTrackingNumberJobCommand>(mediator);