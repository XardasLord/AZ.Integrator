using AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application;
using Hangfire.Console;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignInvoice;

public class AssignInvoiceInShopifyJobCommandHandler(IShopifyService shopifyService)
    : IRequestHandler<AssignInvoiceInShopifyJobCommand>
{
    public ValueTask<Unit> Handle(AssignInvoiceInShopifyJobCommand command, CancellationToken cancellationToken)
    {
        command.PerformContext.Step($"Assigning invoice '{command.InvoiceNumber}' to Allegro order - {command.OrderNumber}");
        
        command.PerformContext.Warning("There is no implementation yet");
        
        return ValueTask.FromResult(Unit.Value);
    }
}