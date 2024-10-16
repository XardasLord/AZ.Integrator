using Hangfire.Server;

namespace AZ.Integrator.Orders.Application.Common.BackgroundJobs;

public abstract class JobCommandBase : Mediator.IRequest
{
    public PerformContext PerformContext { get; set; }
}