using System.Globalization;
using AutoMapper;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;

public class GetAllQueryHandler(
    IAllegroService allegroService,
    IErliService erliService,
    IMapper mapper,
    ICurrentUser currentUser) : IRequestHandler<GetAllQuery, GetAllQueryResponse>
{
    public async ValueTask<GetAllQueryResponse> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        if (currentUser.ShopProviderType == ShopProviderType.Allegro)
        {
            var ordersResponse = await allegroService.GetOrders(query.Filters); 
            
            var orderDtos = mapper.Map<List<OrderDetailsDto>>(ordersResponse.CheckoutForms);
            return new GetAllQueryResponse(orderDtos, ordersResponse.Count, ordersResponse.TotalCount);
        }

        if (currentUser.ShopProviderType == ShopProviderType.Erli)
        {
            var ordersResponse = await erliService.GetOrders(query.Filters);

            var orderDtos = MapErliOrders(ordersResponse);

            return new GetAllQueryResponse(orderDtos, ordersResponse.Count, ordersResponse.TotalCount);
        }

        return null;
    }

    private static List<OrderDetailsDto> MapErliOrders(GetOrdersModelResponse ordersResponse)
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
                    Currency = "PLN"
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
                        Currency = "PLN"
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
                        Currency = "PLN"
                    }
                },
                MessageToSeller = order.Comment
            });
        });
        
        return orderDtos;
    }
}