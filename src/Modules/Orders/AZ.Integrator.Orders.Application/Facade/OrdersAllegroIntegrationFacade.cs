using AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetShopInfo;
using AZ.Integrator.Orders.Contracts;
using AZ.Integrator.Orders.Contracts.Dtos;
using Mediator;

namespace AZ.Integrator.Orders.Application.Facade;

public class OrdersAllegroIntegrationFacade(IMediator mediator) : IOrdersAllegroIntegrationFacade
{
    public async Task<AllegroShopInfoResponseDto> GetShopInfo(Guid tenantId, string accessToken, CancellationToken cancellationToken)
    {
        var query = new GetShopInfoQuery(tenantId, accessToken);
        
        return await mediator.Send(query, cancellationToken);
    }
}