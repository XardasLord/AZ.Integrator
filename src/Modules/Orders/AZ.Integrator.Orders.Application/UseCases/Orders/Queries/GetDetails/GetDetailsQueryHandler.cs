using AutoMapper;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Orders.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Orders.Application.Common.ExternalServices.Erli;
using AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;
using AZ.Integrator.Orders.Application.UseCases.Orders.Extensions;
using AZ.Integrator.Orders.Contracts.Dtos;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetDetails;

public class GetDetailsQueryHandler(
    IAllegroService allegroService,
    IErliService erliService,
    IShopifyService shopifyService,
    IMapper mapper)
    : IRequestHandler<GetDetailsQuery, OrderDetailsDto>
{
    public async ValueTask<OrderDetailsDto> Handle(GetDetailsQuery query, CancellationToken cancellationToken)
    {
        var shopProvider = ShopProviderHelper.GetShopProviderType(query.SourceSystemId);
        
        switch (shopProvider)
        {
            case ShopProviderType.Allegro:
            {
                var orderResponse = await allegroService.GetOrderDetails(new Guid(query.OrderId), query.TenantId, query.SourceSystemId); 

                var orderDetailsDto = orderResponse.MapToDto(mapper);
                
                return orderDetailsDto;
            }
            case ShopProviderType.Erli:
            {
                var orderResponse = await erliService.GetOrderDetails(query.OrderId, query.TenantId, query.SourceSystemId);

                return orderResponse.MapToDto();
            }
            case ShopProviderType.Shopify:
            {
                var ordersResponse = await shopifyService.GetOrderDetails(query.OrderId, query.TenantId, query.SourceSystemId);

                return ordersResponse.MapToDto();
            }
            case ShopProviderType.System:
            case ShopProviderType.Unknown:
            default:
                return null;
        }
    }
}