using AutoMapper;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Orders.Application.Common.ExternalServices.Allegro;
using AZ.Integrator.Orders.Application.Common.ExternalServices.Erli;
using AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;
using AZ.Integrator.Orders.Application.UseCases.Orders.Extensions;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;

public class GetAllQueryHandler(
    IAllegroService allegroService,
    IErliService erliService,
    IShopifyService shopifyService,
    IMapper mapper) : IRequestHandler<GetAllQuery, GetAllQueryResponse>
{
    public async ValueTask<GetAllQueryResponse> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var shopProvider = TenantHelper.GetShopProviderType(query.TenantId);
        
        switch (shopProvider)
        {
            case ShopProviderType.Allegro:
            {
                var ordersResponse = await allegroService.GetOrders(query.Filters, query.TenantId); 
            
                var orderDtos = ordersResponse.CheckoutForms.Select(order => order.MapToDto(mapper));
                
                return new GetAllQueryResponse(orderDtos, ordersResponse.Count, ordersResponse.TotalCount);
            }
            case ShopProviderType.Erli:
            {
                var ordersResponse = await erliService.GetOrders(query.Filters, query.TenantId);

                var orderDtos = ordersResponse.Orders.Select(order => order.MapToDto());

                return new GetAllQueryResponse(orderDtos, ordersResponse.Count, ordersResponse.TotalCount);
            }
            case ShopProviderType.Shopify:
            {
                var ordersResponse = await shopifyService.GetOrders(query.Filters, query.TenantId);

                var orderDtos = ordersResponse.Orders.Select(order => order.MapToDto());

                return new GetAllQueryResponse(orderDtos, ordersResponse.Count, ordersResponse.TotalCount);
            }
            case ShopProviderType.System:
            case ShopProviderType.Unknown:
            default:
                return null;
        }
    }
}