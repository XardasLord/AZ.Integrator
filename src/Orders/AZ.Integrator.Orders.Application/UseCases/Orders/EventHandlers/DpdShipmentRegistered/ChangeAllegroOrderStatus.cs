using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeStatus;
using Hangfire;
using MediatR;
using DpdShipmentRegisteredEvent = AZ.Integrator.Shipments.Domain.Events.DomainEvents.DpdShipment.DpdShipmentRegistered;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.DpdShipmentRegistered;

public class ChangeAllegroOrderStatus : INotificationHandler<DpdShipmentRegisteredEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ICurrentUser _currentUser;

    public ChangeAllegroOrderStatus(IBackgroundJobClient backgroundJobClient, ICurrentUser currentUser)
    {
        _backgroundJobClient = backgroundJobClient;
        _currentUser = currentUser;
    }
    
    public Task Handle(DpdShipmentRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<ChangeStatusJob>(
            job => job.Execute(new ChangeStatusJobCommand
            {
                OrderNumber = Guid.Parse(notification.AllegroOrderNumber),
                AllegroAccessToken = _currentUser.AllegroAccessToken
            }, null));
        
        return Task.CompletedTask;
    }
}