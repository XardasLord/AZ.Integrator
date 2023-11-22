using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Orders.Application.UseCases.Orders.JobCommands.ChangeStatus;
using Hangfire;
using MediatR;
using InpostShipmentRegisteredEvent = AZ.Integrator.Shipments.Domain.Events.DomainEvents.InpostShipment.InpostShipmentRegistered;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.EventHandlers.InpostShipmentRegistered;

public class ChangeAllegroOrderStatus : INotificationHandler<InpostShipmentRegisteredEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ICurrentUser _currentUser;

    public ChangeAllegroOrderStatus(IBackgroundJobClient backgroundJobClient, ICurrentUser currentUser)
    {
        _backgroundJobClient = backgroundJobClient;
        _currentUser = currentUser;
    }
    
    public Task Handle(InpostShipmentRegisteredEvent notification, CancellationToken cancellationToken)
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