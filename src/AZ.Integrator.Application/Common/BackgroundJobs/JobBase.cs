﻿using Hangfire.Server;
using Mediator;

namespace AZ.Integrator.Application.Common.BackgroundJobs;

public class JobBase<T> where T : JobCommandBase, ICommand
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

    protected virtual ValueTask<Unit> ExecuteCommand(T command) => Mediator.Send(command);
}