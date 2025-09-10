using System.Globalization;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder.Strategy;

public sealed class ErliInvoiceDraftBuilder(IErliService erliService) : IInvoiceDraftBuilder
{
    public ShopProviderType Provider => ShopProviderType.Erli;
    
    public async Task<GenerateInvoiceRequest> BuildAsync(string externalOrderNumber, string tenantId, CancellationToken cancellationToken)
    {
        var orderDetails = await erliService.GetOrderDetails(externalOrderNumber, tenantId);

        var address = string.Empty;

        address = BuildAddress(orderDetails, address);
        
        var buyerDetails = new BuyerDto(
            orderDetails.User?.Email,
            orderDetails.User?.InvoiceAddress?.FirstName ?? orderDetails.User?.DeliveryAddress?.FirstName,
            orderDetails.User?.InvoiceAddress?.LastName ?? orderDetails.User?.DeliveryAddress?.LastName,
            orderDetails.User?.InvoiceAddress?.CompanyName,
            orderDetails.User?.InvoiceAddress?.Nip,
            address,
            orderDetails.User?.InvoiceAddress?.City ?? orderDetails.User?.DeliveryAddress?.City,
            orderDetails.User?.InvoiceAddress?.Zip ?? orderDetails.User?.DeliveryAddress?.Zip,
            orderDetails.User?.InvoiceAddress?.Country ?? orderDetails.User?.DeliveryAddress?.Country);

        var invoiceItems = orderDetails.Items.Select(x =>
                new InvoiceLineDto(x.Name,
                    decimal.Parse((x.UnitPrice / 100m).ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                    x.Quantity,
                    "PL"))
            .ToList();

        var paymentDetails = new PaymentTermsDto(
            orderDetails.PurchasedAt.Date,
            orderDetails.PurchasedAt.Date,
            DateTime.UtcNow,
            !orderDetails.Delivery.Cod);

        var deliveryDetails = new DeliveryDto(
            orderDetails.Delivery.Name,
            decimal.Parse(((orderDetails.Delivery?.Price ?? 0) / 100m).ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        
        return new GenerateInvoiceRequest(
            BuyerDto: buyerDetails,
            InvoiceLines: invoiceItems,
            PaymentTermsDto: paymentDetails,
            DeliveryDto: deliveryDetails,
            IdempotencyKey: $"{tenantId}:{Provider}:order:{externalOrderNumber}",
            externalOrderNumber,
            tenantId);
    }

    private static string BuildAddress(Order orderDetails, string address)
    {
        if (orderDetails.User.InvoiceAddress is not null)
        {
            var invoiceAddress = orderDetails.User.InvoiceAddress;
            address = $"{invoiceAddress.Street} {invoiceAddress.BuildingNumber}{(string.IsNullOrWhiteSpace(invoiceAddress.FlatNumber) ? "" : $" / {invoiceAddress.FlatNumber}")}";
        }
        else if (orderDetails.User.DeliveryAddress is not null)
        {
            var deliveryAddress = orderDetails.User.DeliveryAddress;
            address = $"{deliveryAddress.Street} {deliveryAddress.BuildingNumber}{(string.IsNullOrWhiteSpace(deliveryAddress.FlatNumber) ? "" : $" / {deliveryAddress.FlatNumber}")}";
        }

        return address;
    }
}