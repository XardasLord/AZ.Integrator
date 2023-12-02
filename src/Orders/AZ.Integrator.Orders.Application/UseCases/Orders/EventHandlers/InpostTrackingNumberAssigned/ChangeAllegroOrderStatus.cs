using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeAllegroOrderStatus;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using Hangfire;
using MediatR;
using InpostTrackingNumberAssignedEvent = AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment.InpostTrackingNumberAssigned;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InpostTrackingNumberAssigned;

public class ChangeAllegroOrderStatus : INotificationHandler<InpostTrackingNumberAssignedEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public ChangeAllegroOrderStatus(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    public Task Handle(InpostTrackingNumberAssignedEvent notification, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<ChangeAllegroOrderStatusJob>(
            job => job.Execute(new ChangeAllegroOrderStatusJobCommand
            {
                OrderNumber = Guid.Parse(notification.AllegroOrderNumber),
                OrderStatus = AllegroFulfillmentStatusEnum.ReadyForShipment,
                AllegroAccessToken = notification.AllegroAccessToken
            }, null));
        
        return Task.CompletedTask;
    }
}