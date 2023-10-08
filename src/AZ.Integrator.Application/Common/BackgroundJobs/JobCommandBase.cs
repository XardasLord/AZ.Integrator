using Hangfire.Server;
using Mediator;

namespace AZ.Integrator.Application.Common.BackgroundJobs;

public abstract class JobCommandBase : ICommand
{
    public PerformContext PerformContext { get; set; }
}