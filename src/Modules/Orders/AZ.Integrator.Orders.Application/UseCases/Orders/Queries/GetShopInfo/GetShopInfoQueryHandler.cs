using AZ.Integrator.Orders.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Orders.Contracts.Dtos;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetShopInfo;

public class GetShopInfoQueryHandler(IAllegroService allegroService) : IRequestHandler<GetShopInfoQuery, AllegroShopInfoResponseDto>
{
    public async ValueTask<AllegroShopInfoResponseDto> Handle(GetShopInfoQuery query, CancellationToken cancellationToken)
    {
        var orderResponse = await allegroService.GetShopInfo(query.TenantId, query.AccessToken); 

        // var orderDetailsDto = orderResponse.MapToDto(mapper);
                
        return orderResponse;
    }
}