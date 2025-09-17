using System.Globalization;
using AutoMapper;
using AZ.Integrator.Orders.Contracts.Dtos;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AllegroOrder = AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models.GetOrderDetailsModelResponse;
using ErliOrder = AZ.Integrator.Shared.Application.ExternalServices.Erli.Order;
using ShopifyOrder = AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses.Order;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Extensions;

public static class OrderResponseExtensions
{
    public static OrderDetailsDto MapToDto(this AllegroOrder order, IMapper mapper)
    {
        var orderDto = mapper.Map<OrderDetailsDto>(order);
        
        orderDto.Delivery.Cod = orderDto.Payment.Type == OrderPaymentType.Cod;
        orderDto.PurchasedAt = orderDto.Payment.FinishedAt ?? order.UpdatedAt;

        return orderDto;
    }
    
    public static OrderDetailsDto MapToDto(this ErliOrder order)
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

        var orderDto = new OrderDetailsDto
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
                PaidAmount = order.Payment is null
                    ? null
                    : new AmountDetailsDto
                    {
                        Amount = (order.TotalPrice / 100m).ToString(CultureInfo.InvariantCulture),
                        Currency = CurrencyEnum.Pln.Name
                    },
                Type = order.Delivery.Cod ? OrderPaymentType.Cod : OrderPaymentType.Online,
                FinishedAt = order.PurchasedAt
            },
            Delivery = new DeliveryDetailsDto
            {
                Method = new MethodDetailsDto
                {
                    Name = order.Delivery.Name
                },
                Address = new DeliveryAddressDetailsDto
                {
                    CompanyName = null,
                    FirstName = order.User.DeliveryAddress.FirstName,
                    LastName = order.User.DeliveryAddress.LastName,
                    City = order.User.DeliveryAddress.City,
                    Street =
                        $"{order.User.DeliveryAddress.Street} {order.User.DeliveryAddress.BuildingNumber}{(string.IsNullOrEmpty(order.User.DeliveryAddress.FlatNumber) ? "" : $"/{order.User.DeliveryAddress.FlatNumber}")}",
                    ZipCode = order.User.DeliveryAddress.Zip,
                    PhoneNumber = order.User.DeliveryAddress.Phone,
                    CountryCode = order.User.DeliveryAddress.Country.ToUpper()
                },
                Cod = order.Delivery.Cod,
                Cost = new AmountDetailsDto
                {
                    Amount = (order.Delivery?.Price ?? 0 / 100m).ToString(CultureInfo.InvariantCulture),
                    Currency = CurrencyEnum.Pln.Name
                }
            },
            Invoice = new InvoiceDetailsDto
            {
                Required = order.User.InvoiceAddress is not null,
                DueDate = order.PurchasedAt.Date.ToString("yyyy-MM-dd"),
                Address = new AddressDetailsDto
                {
                    Street = order.User.InvoiceAddress is null
                        ? null
                        : $"{order.User.InvoiceAddress.Street} {order.User.InvoiceAddress.BuildingNumber}{(string.IsNullOrWhiteSpace(order.User.InvoiceAddress.FlatNumber) ? "" : $" / {order.User.InvoiceAddress.FlatNumber}")}",
                    City = order.User.InvoiceAddress?.City,
                    ZipCode = order.User.InvoiceAddress?.Zip,
                    CountryCode = order.User.InvoiceAddress?.Country,
                    Company = order.User.InvoiceAddress is null
                        ? null
                        : new InvoiceCompanyDetailsDto
                        {
                            Name = order.User.InvoiceAddress.CompanyName,
                            TaxId = order.User.InvoiceAddress.Nip
                        },
                    NaturalPerson = order.User.InvoiceAddress is null
                        ? null
                        : new InvoiceNaturalPersonDetailsDto
                        {
                            FirstName = order.User.InvoiceAddress.FirstName,
                            LastName = order.User.InvoiceAddress.LastName
                        }
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
        };
        
        return orderDto;
    }
    
    public static OrderDetailsDto MapToDto(this ShopifyOrder order)
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

        var orderDto = new OrderDetailsDto
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
                PaidAmount = order.FullyPaid
                    ? new AmountDetailsDto
                    {
                        Amount = order.TotalPriceSet?.PresentmentMoney?.Amount,
                        Currency = order.TotalPriceSet?.PresentmentMoney?.CurrencyCode
                    }
                    : null,
                Type = order.FullyPaid ? OrderPaymentType.Online : OrderPaymentType.Cod,
                FinishedAt = order.CreatedAt.Date
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
                    Street = string.IsNullOrWhiteSpace(order.ShippingAddress?.Address1)
                        ? null
                        : $"{order.ShippingAddress?.Address1} {order.ShippingAddress?.Address2}",
                    ZipCode = order.ShippingAddress?.Zip,
                    PhoneNumber = order.ShippingAddress?.Phone,
                    CountryCode = order.ShippingAddress?.CountryCodeV2
                },
                Cod = !order.FullyPaid,
                Cost = new AmountDetailsDto
                {
                    Amount = order.ShippingLine?.CurrentDiscountedPriceSet?.PresentmentMoney?.Amount ?? 0.ToString(),
                    Currency = order.ShippingLine?.CurrentDiscountedPriceSet?.PresentmentMoney?.CurrencyCode
                }
            },
            Invoice = order.BillingAddress is null
                ? null
                : new InvoiceDetailsDto
                {
                    Required = order.BillingAddress is not null,
                    DueDate = order.CreatedAt.Date.ToString("yyyy-MM-dd"),
                    Address = new AddressDetailsDto
                    {
                        Street = $"{order.BillingAddress.Address1} {order.BillingAddress.Address2}",
                        City = order.BillingAddress.City,
                        ZipCode = order.BillingAddress.Zip,
                        CountryCode = order.BillingAddress.CountryCodeV2,
                        Company = new InvoiceCompanyDetailsDto
                        {
                            Name = order.BillingAddress.Company,
                            TaxId = null
                        },
                        NaturalPerson = new InvoiceNaturalPersonDetailsDto
                        {
                            FirstName = order.BillingAddress.FirstName,
                            LastName = order.BillingAddress.LastName
                        }
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
            MessageToSeller = order.Note,
        };

        return orderDto;
    }
}
