using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeAllegroOrderStatus;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InpostTrackingNumberAssigned;

public class ChangeAllegroOrderStatus : INotificationHandler<InpostTrackingNumbersAssigned>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public ChangeAllegroOrderStatus(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    public ValueTask Handle(InpostTrackingNumbersAssigned notification, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<ChangeAllegroOrderStatusJob>(
            job => job.Execute(new ChangeAllegroOrderStatusJobCommand
            {
                OrderNumber = Guid.Parse(notification.AllegroOrderNumber),
                OrderStatus = AllegroFulfillmentStatusEnum.ReadyForShipment.Value,
                TenantId = notification.TenantId
            }, null));
        
        return new ValueTask();
    }
}