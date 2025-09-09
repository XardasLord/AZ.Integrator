using System.Globalization;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder.Strategy;

public sealed class ShopifyInvoiceDraftBuilder(IShopifyService shopifyService) : IInvoiceDraftBuilder
{
    public ShopProviderType Provider => ShopProviderType.Shopify;
    
    public async Task<GenerateInvoiceRequest> BuildAsync(string externalOrderNumber, string tenantId, CancellationToken ct)
    {
        var orderDetails = await shopifyService.GetOrderDetails(externalOrderNumber, tenantId);
        
        var buyerDetails = new BuyerDto(
            orderDetails.Email,
            orderDetails.BillingAddress?.FirstName,
            orderDetails.BillingAddress?.LastName,
            orderDetails.BillingAddress?.Company,
            null,
            orderDetails.BillingAddress?.Address1 + " " + orderDetails.BillingAddress?.Address2,
            orderDetails.BillingAddress?.City,
            orderDetails.BillingAddress?.Zip,
            orderDetails.BillingAddress?.Country);

        var invoiceItems = orderDetails.LineItems.Nodes.Select(x =>
                new InvoiceLineDto(x.Name,
                    decimal.Parse((string)x.OriginalUnitPriceSet.PresentmentMoney.Amount, CultureInfo.InvariantCulture),
                    x.Quantity,
                    x.OriginalUnitPriceSet.PresentmentMoney.CurrencyCode))
            .ToList();

        var paymentDetails = new PaymentTermsDto(
            orderDetails.CreatedAt.Date,
            orderDetails.CreatedAt.Date,
            DateTime.UtcNow,
            orderDetails.FullyPaid);

        var deliveryDetails = new DeliveryDto(
            orderDetails.ShippingLine.Title,
            decimal.Parse(orderDetails.ShippingLine?.CurrentDiscountedPriceSet?.PresentmentMoney?.Amount ?? 0.ToString(),
                CultureInfo.InvariantCulture));
        
        return new GenerateInvoiceRequest(
            BuyerDto: buyerDetails,
            InvoiceLines: invoiceItems,
            PaymentTermsDto: paymentDetails,
            DeliveryDto: deliveryDetails,
            IdempotencyKey: $"{tenantId}:{Provider}:order:{externalOrderNumber}",
            externalOrderNumber,
            tenantId);
    }
}