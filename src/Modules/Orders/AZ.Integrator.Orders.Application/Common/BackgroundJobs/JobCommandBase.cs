using Hangfire.Server;
using Mediator;

namespace AZ.Integrator.Orders.Application.Common.BackgroundJobs;

public abstract class JobCommandBase : IRequest
{
    public PerformContext PerformContext { get; set; }
}