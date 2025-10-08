using AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignTrackingNumbers;

public class AssignTrackingNumbersInShopifyJobCommandHandler(IShopifyService shopifyService)
    : IRequestHandler<AssignTrackingNumbersInShopifyJobCommand>
{
    public async ValueTask<Unit> Handle(AssignTrackingNumbersInShopifyJobCommand command, CancellationToken cancellationToken)
    {
        await shopifyService.AssignTrackingNumber(
            command.OrderNumber,
            command.TrackingNumbers,
            command.Vendor,
            command.TenantId,
            command.SourceSystemId);
        
        return Unit.Value;
    }
}