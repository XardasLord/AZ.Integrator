using System.Globalization;
using AutoMapper;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
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
        switch (query.ShopProvider)
        {
            case ShopProviderType.Allegro:
            {
                var ordersResponse = await allegroService.GetOrders(query.Filters, query.TenantId); 
            
                var orderDtos = mapper.Map<List<OrderDetailsDto>>(ordersResponse.CheckoutForms);
                return new GetAllQueryResponse(orderDtos, ordersResponse.Count, ordersResponse.TotalCount);
            }
            case ShopProviderType.Erli:
            {
                var ordersResponse = await erliService.GetOrders(query.Filters, query.TenantId);

                var orderDtos = MapErliOrders(ordersResponse);

                return new GetAllQueryResponse(orderDtos, ordersResponse.Count, ordersResponse.TotalCount);
            }
            case ShopProviderType.Shopify:
            {
                var ordersResponse = await shopifyService.GetOrders(query.Filters, query.TenantId);

                var orderDtos = MapShopifyOrders(ordersResponse);

                return new GetAllQueryResponse(orderDtos, ordersResponse.Count, ordersResponse.TotalCount);
            }
            case ShopProviderType.System:
            case ShopProviderType.Unknown:
            default:
                return null;
        }
    }

    private static List<OrderDetailsDto> MapErliOrders(AZ.Integrator.Shared.Application.ExternalServices.Erli.GetOrdersModelResponse ordersResponse)
    {
        List<OrderDetailsDto> orderDtos = [];

        ordersResponse.Orders.ToList().ForEach(order =>
        {
            var lineItems = order.Items.Select(item => new LineItemDetailsDto
            {
                Offer = new OfferDetailsDto
                {
                    Name = item.Name,
                    External = string.IsNullOrEmpty(item.Sku) ? null : new ExternalDetailsDto
                    {
                        Id = item.Sku
                    }
                },
                Quantity = item.Quantity,
                Price = new AmountDetailsDto
                {
                    Amount = (item.UnitPrice / 100m).ToString(CultureInfo.InvariantCulture),
                    Currency = CurrencyEnum.Pln.Name
                }
            }).ToList();
            
            orderDtos.Add(new OrderDetailsDto
            {
                Id = order.Id,
                Status = order.Status,
                UpdatedAt = order.Created,
                Buyer = new BuyerDetailsDto
                {
                    Login = order.User.Email,
                    FirstName = order.User.DeliveryAddress.FirstName,
                    LastName = order.User.DeliveryAddress.LastName,
                    Email = order.User.Email,
                },
                Payment = new PaymentDetailsDto
                {
                    PaidAmount = order.Payment is null ? null : new AmountDetails
                    {
                        Amount = (order.TotalPrice / 100m).ToString(CultureInfo.InvariantCulture),
                        Currency = CurrencyEnum.Pln.Name
                    },
                    Type = order.Delivery.Cod ? OrderPaymentType.Cod : OrderPaymentType.Online,
                },
                Delivery = new DeliveryDetailsDto
                {
                    Method = new MethodDetailsDto
                    {
                        Name = order.Delivery.Name
                    },
                    Address = new DeliveryAddressDetailsDto
                    {
                        FirstName = order.User.DeliveryAddress.FirstName,
                        LastName = order.User.DeliveryAddress.LastName,
                        City = order.User.DeliveryAddress.City,
                        Street = $"{order.User.DeliveryAddress.Street} {order.User.DeliveryAddress.BuildingNumber}{(string.IsNullOrEmpty(order.User.DeliveryAddress.FlatNumber) ? "" : $"/{order.User.DeliveryAddress.FlatNumber}")}",
                        ZipCode = order.User.DeliveryAddress.Zip,
                        PhoneNumber = order.User.DeliveryAddress.Phone,
                        CountryCode = order.User.DeliveryAddress.Country.ToUpper()
                    }
                },
                LineItems = lineItems,
                Summary = new SummaryDetailsDto
                {
                    TotalToPay = new AmountDetailsDto
                    {
                        Amount = (order.TotalPrice / 100m).ToString(CultureInfo.InvariantCulture),
                        Currency = CurrencyEnum.Pln.Name
                    }
                },
                MessageToSeller = order.Comment
            });
        });
        
        return orderDtos;
    }
    

    private static List<OrderDetailsDto> MapShopifyOrders(AZ.Integrator.Shared.Application.ExternalServices.Shopify.GetOrdersModelResponse ordersResponse)
    {
        List<OrderDetailsDto> orderDtos = [];
        
        ordersResponse.Orders.ToList().ForEach(order =>
        {
            var lineItems = order.LineItems.Nodes.Select(item => new LineItemDetailsDto
            {
                Offer = new OfferDetailsDto
                {
                    Name = item.Name,
                    External = string.IsNullOrEmpty(item.Sku) ? null : new ExternalDetailsDto
                    {
                        Id = item.Sku
                    }
                },
                Quantity = item.Quantity,
                Price = new AmountDetailsDto
                {
                    Amount = item.OriginalUnitPriceSet?.PresentmentMoney?.Amount,
                    Currency = item.OriginalUnitPriceSet?.PresentmentMoney?.CurrencyCode,
                }
            }).ToList();
            
            orderDtos.Add(new OrderDetailsDto
            {
                Id = order.Name, // ID = gid://shopify/Order/7011438395732, so the name (readable number) is used here for better later reference
                UpdatedAt = order.CreatedAt.LocalDateTime,
                // Status = order.Status,
                Buyer = new BuyerDetailsDto
                {
                    // Login = order.User.Email,
                    // FirstName = order.User.DeliveryAddress.FirstName,
                    // LastName = order.User.DeliveryAddress.LastName,
                    // Email = order.User.Email,
                },
                Payment = new PaymentDetailsDto
                {
                    PaidAmount = order.FullyPaid ? new AmountDetails
                    {
                        Amount = order.TotalPriceSet?.PresentmentMoney?.Amount,
                        Currency = order.TotalPriceSet?.PresentmentMoney?.CurrencyCode
                    } : null,
                    Type = order.FullyPaid ? OrderPaymentType.Online : OrderPaymentType.Cod
                },
                Delivery = new DeliveryDetailsDto
                {
                    Method = new MethodDetailsDto
                    {
                        Name = order.ShippingLine?.Title
                    },
                    Address = new DeliveryAddressDetailsDto
                    {
                        FirstName = order.ShippingAddress?.FirstName,
                        LastName = order.ShippingAddress?.LastName,
                        City = order.ShippingAddress?.City,
                        Street = string.IsNullOrWhiteSpace(order.ShippingAddress?.Address1) ? null : $"{order.ShippingAddress?.Address1} {order.ShippingAddress?.Address2}",
                        ZipCode = order.ShippingAddress?.Zip,
                        PhoneNumber = order.ShippingAddress?.Phone,
                        CountryCode = order.ShippingAddress?.CountryCodeV2
                    }
                },
                LineItems = lineItems,
                Summary = new SummaryDetailsDto
                {
                    TotalToPay = new AmountDetailsDto
                    {
                        Amount = order.TotalPriceSet?.PresentmentMoney?.Amount,
                        Currency = order.TotalPriceSet?.PresentmentMoney?.CurrencyCode
                    }
                },
                MessageToSeller = order.Note
            });
        });
        
        return orderDtos;
    }
}