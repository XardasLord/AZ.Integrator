using AZ.Integrator.Orders.Contracts.Dtos;

namespace AZ.Integrator.Orders.Contracts;

public interface IOrdersFacade
{
    Task<OrderDetailsDto> GetOrderDetails(string orderId, Guid tenantId, string sourceSystemId, CancellationToken cancellationToken);
}