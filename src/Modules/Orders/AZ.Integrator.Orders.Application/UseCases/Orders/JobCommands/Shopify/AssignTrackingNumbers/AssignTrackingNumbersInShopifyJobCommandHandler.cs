using AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.Shopify.AssignTrackingNumbers;

public class AssignTrackingNumbersInShopifyJobCommandHandler(IShopifyService shopifyService)
    : IRequestHandler<AssignTrackingNumbersInShopifyJobCommand>
{
    public async ValueTask<Unit> Handle(AssignTrackingNumbersInShopifyJobCommand command, CancellationToken cancellationToken)
    {
        command.PerformContext.Step("Starting assigning tracking numbers in Shopify...");
        
        await shopifyService.AssignTrackingNumber(
            command.OrderNumber,
            command.TrackingNumbers,
            command.Vendor,
            command.TenantId,
            command.SourceSystemId);
        
        command.PerformContext.Success("Tracking numbers assigned successfully in Shopify");
        
        return Unit.Value;
    }
}