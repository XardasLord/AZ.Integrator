using AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetDetails;
using AZ.Integrator.Orders.Contracts;
using AZ.Integrator.Orders.Contracts.Dtos;
using Mediator;

namespace AZ.Integrator.Orders.Application.Facade;

public class OrdersFacade(IMediator mediator) : IOrdersFacade
{
    public async Task<OrderDetailsDto> GetOrderDetails(string orderId, string tenantId, CancellationToken cancellationToken)
    {
        var query = new GetDetailsQuery(orderId)
        {
            TenantId = tenantId
        };
        
        return await mediator.Send(query, cancellationToken);
    }
}