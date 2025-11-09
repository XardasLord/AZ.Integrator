using AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands.SendOrderEmailToSupplier;
using AZ.Integrator.Procurement.Domain.Events;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.EventHandlers.OrderRegistered;

public class SendOrderEmailToSupplierEventHandler(IBackgroundJobClient backgroundJobClient)
    : INotificationHandler<PartDefinitionsOrderRegistered>
{
    public ValueTask Handle(PartDefinitionsOrderRegistered notification, CancellationToken cancellationToken)
    {
        backgroundJobClient.Enqueue<SendOrderEmailToSupplierJob>(
            job => job.Execute(new SendOrderEmailToSupplierJobCommand
            {
                SupplierId = notification.SupplierId,
                OrderNumber = notification.OrderNumber,
                TenantId = notification.TenantId
            }, null));

        return ValueTask.CompletedTask;
    }
}

