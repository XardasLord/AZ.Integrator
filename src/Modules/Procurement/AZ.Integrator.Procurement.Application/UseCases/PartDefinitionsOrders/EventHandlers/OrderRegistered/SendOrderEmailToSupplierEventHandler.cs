using AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands;
using AZ.Integrator.Procurement.Domain.Events;
using Hangfire;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.EventHandlers.OrderRegistered;

public class SendOrderEmailToSupplierEventHandler(IBackgroundJobClient backgroundJobClient)
    : INotificationHandler<PartDefinitionsOrderRegistered>
{
    public ValueTask Handle(PartDefinitionsOrderRegistered notification, CancellationToken cancellationToken)
    {
        backgroundJobClient.Enqueue<SendPartDefinitionOrderEmailToSupplierJob>(
            job => job.Execute(new SendPartDefinitionOrderEmailToSupplierJobCommand
            {
                SupplierId = notification.SupplierId,
                OrderNumber = notification.OrderNumber,
                TenantId = notification.TenantId
            }, null));

        return ValueTask.CompletedTask;
    }
}

