using AZ.Integrator.Orders.Application.Common.ExternalServices.Allegro;
using Hangfire.Console;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Allegro.AssignInvoice;

public class AssignInvoiceInAllegroJobCommandHandler(IAllegroService allegroService)
    : IRequestHandler<AssignInvoiceInAllegroJobCommand>
{
    public ValueTask<Unit> Handle(AssignInvoiceInAllegroJobCommand command, CancellationToken cancellationToken)
    {
        command.PerformContext.WriteLine($"Assigning invoice '{command.InvoiceNumber}' to Allegro order - {command.OrderNumber}");
        
        command.PerformContext.SetTextColor(ConsoleTextColor.DarkGreen);
        command.PerformContext.WriteLine("There is no implementation yet");
        
        // await allegroService.AssignTrackingNumber(command.OrderNumber, command.TrackingNumbers, command.TenantId);
        
        return ValueTask.FromResult(Unit.Value);
    }
}