using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignInvoice;

public class AssignInvoiceInShopifyJobCommandHandler(IShopifyService shopifyService)
    : IRequestHandler<AssignInvoiceInShopifyJobCommand>
{
    public ValueTask<Unit> Handle(AssignInvoiceInShopifyJobCommand command, CancellationToken cancellationToken)
    {
        // await allegroService.AssignTrackingNumber(command.OrderNumber, command.TrackingNumbers, command.TenantId);
        
        return ValueTask.FromResult(Unit.Value);
    }
}