using AZ.Integrator.Application.UseCases.Orders.JobCommands.ChangeStatus;
using AZ.Integrator.Domain.Abstractions;
using Hangfire;
using Mediator;
using DpdShipmentRegisteredEvent = AZ.Integrator.Domain.Aggregates.DpdShipment.DomainEvents.DpdShipmentRegistered;

namespace AZ.Integrator.Application.UseCases.Orders.EventHandlers.DpdShipmentRegistered;

public class ChangeAllegroOrderStatus : INotificationHandler<DpdShipmentRegisteredEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ICurrentUser _currentUser;

    public ChangeAllegroOrderStatus(IBackgroundJobClient backgroundJobClient, ICurrentUser currentUser)
    {
        _backgroundJobClient = backgroundJobClient;
        _currentUser = currentUser;
    }
    
    public ValueTask Handle(DpdShipmentRegisteredEvent notification, CancellationToken cancellationToken)
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