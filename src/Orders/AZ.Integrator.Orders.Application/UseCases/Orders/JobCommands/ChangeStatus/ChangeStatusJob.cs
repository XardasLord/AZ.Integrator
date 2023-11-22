﻿using AZ.Integrator.Orders.Application.Common.BackgroundJobs;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeStatus;

public class ChangeStatusJob : JobBase<ChangeStatusJobCommand>
{
    public ChangeStatusJob(IMediator mediator) : base(mediator)
    {
    }
}