﻿using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignTrackingNumbers;

public class AssignTrackingNumbersInAllegroJobCommandHandler(IAllegroService allegroService)
    : IRequestHandler<AssignTrackingNumbersInAllegroJobCommand>
{
    public async ValueTask<Unit> Handle(AssignTrackingNumbersInAllegroJobCommand command, CancellationToken cancellationToken)
    {
        await allegroService.AssignTrackingNumber(command.OrderNumber, command.TrackingNumbers, command.TenantId);
        
        return Unit.Value;
    }
}