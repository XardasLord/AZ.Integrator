using Hangfire.Server;
using MediatR;

namespace AZ.Integrator.Shipments.Application.Common.BackgroundJobs;

public class JobBase<T> where T : JobCommandBase, IRequest
{
    protected readonly IMediator Mediator;
    
    protected JobBase(IMediator mediator)
    {
        Mediator = mediator;
    }
    
    public async Task Execute(T command, PerformContext context)
    {
        command.PerformContext = context;
        await ExecuteCommand(command);
    }

    protected virtual Task<Unit> ExecuteCommand(T command) => Mediator.Send(command);
}