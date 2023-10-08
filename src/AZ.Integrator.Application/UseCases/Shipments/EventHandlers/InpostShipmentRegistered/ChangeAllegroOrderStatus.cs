using AZ.Integrator.Application.UseCases.Orders.JobCommands.ChangeStatus;
using AZ.Integrator.Domain.Abstractions;
using Hangfire;
using Mediator;
using InpostShipmentRegisteredEvent = AZ.Integrator.Domain.Aggregates.InpostShipment.DomainEvents.InpostShipmentRegistered;

namespace AZ.Integrator.Application.UseCases.Shipments.EventHandlers.InpostShipmentRegistered;

public class ChangeAllegroOrderStatus : INotificationHandler<InpostShipmentRegisteredEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ICurrentUser _currentUser;

    public ChangeAllegroOrderStatus(IBackgroundJobClient backgroundJobClient, ICurrentUser currentUser)
    {
        _backgroundJobClient = backgroundJobClient;
        _currentUser = currentUser;
    }
    
    public ValueTask Handle(InpostShipmentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<ChangeStatusJob>(
            job => job.Execute(new ChangeStatusJobCommand
            {
                OrderNumber = Guid.Parse(notification.AllegroOrderNumber.Value),
                AllegroAccessToken = _currentUser.AllegroAccessToken
            }, null));
        
        return ValueTask.CompletedTask;
    }
}