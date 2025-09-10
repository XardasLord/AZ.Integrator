using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;
using Hangfire.Console;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignInvoice;

public class AssignInvoiceInShopifyJobCommandHandler(IShopifyService shopifyService)
    : IRequestHandler<AssignInvoiceInShopifyJobCommand>
{
    public ValueTask<Unit> Handle(AssignInvoiceInShopifyJobCommand command, CancellationToken cancellationToken)
    {
        command.PerformContext.WriteLine($"Assigning invoice '{command.InvoiceNumber}' to Allegro order - {command.OrderNumber}");
        
        command.PerformContext.SetTextColor(ConsoleTextColor.DarkGreen);
        command.PerformContext.WriteLine("There is no implementation yet");
        
        return ValueTask.FromResult(Unit.Value);
    }
}