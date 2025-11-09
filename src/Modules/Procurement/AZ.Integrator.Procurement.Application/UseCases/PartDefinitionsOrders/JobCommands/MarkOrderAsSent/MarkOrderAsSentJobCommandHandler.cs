using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Procurement.Application.Common.BackgroundJobs;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.Specifications;
using Hangfire.Server;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.JobCommands.MarkOrderAsSent;

public class MarkOrderAsSentJobCommandHandler(
    IAggregateRepository<PartDefinitionsOrder> repository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime)
    : IRequestHandler<MarkOrderAsSentJobCommand>
{
    private PerformContext _ctx = null!;
    
    public async ValueTask<Unit> Handle(MarkOrderAsSentJobCommand command, CancellationToken cancellationToken)
    {
        _ctx = command.PerformContext;

        cancellationToken.ThrowIfCancellationRequested();

        var order = await LoadOrderAsync(command, cancellationToken);
        if (order is null) return Unit.Value;

        await SetOrderAsSent(order, cancellationToken);
        
        return Unit.Value;
    }

    private async Task<PartDefinitionsOrder?> LoadOrderAsync(
        MarkOrderAsSentJobCommand command,
        CancellationToken cancellationToken)
    {
        _ctx.Step($"Starting loading order '{command.OrderNumber}' for tenant {command.TenantId}...");

        var spec = new OrderByNumberSpec(command.OrderNumber, command.TenantId);
        var order = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        if (order is null)
        {
            _ctx.Error($"Order '{command.OrderNumber}' not found for tenant {command.TenantId}");
            return null;
        }

        _ctx.Success("Order loaded successfully");
        return order;
    }
    
    private async Task SetOrderAsSent(PartDefinitionsOrder order, CancellationToken cancellationToken)
    {
        _ctx.Step("Marking order as sent...");

        order.MarkAsSent(currentUser, currentDateTime);

        await repository.SaveChangesAsync(cancellationToken);
        
        _ctx.Success("Order marked as sent successfully");
    }
}

