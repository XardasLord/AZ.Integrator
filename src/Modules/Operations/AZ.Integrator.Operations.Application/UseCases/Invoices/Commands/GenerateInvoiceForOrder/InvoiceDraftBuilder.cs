using System.Globalization;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Orders.Contracts;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;

public sealed class InvoiceDraftBuilder(IOrdersFacade ordersFacade) : IInvoiceDraftBuilder
{
    public async Task<GenerateInvoiceRequest> BuildAsync(
        string externalOrderNumber,
        Guid tenantId,
        string sourceSystemId,
        string correlationKey,
        CancellationToken cancellationToken)
    {
        var orderDetails = await ordersFacade.GetOrderDetails(externalOrderNumber, tenantId, sourceSystemId, cancellationToken);
        
        var buyerDetails = new BuyerDto(
            orderDetails.Buyer.Email,
            orderDetails.Invoice?.Address?.NaturalPerson?.FirstName ?? orderDetails.Delivery?.Address?.FirstName ?? orderDetails.Buyer.FirstName,
            orderDetails.Invoice?.Address?.NaturalPerson?.LastName ?? orderDetails.Delivery?.Address?.LastName ?? orderDetails.Buyer.LastName,
            orderDetails.Invoice?.Address?.Company?.Name ?? orderDetails.Delivery?.Address?.CompanyName,
            orderDetails.Invoice?.Address?.Company?.TaxId,
            orderDetails.Invoice?.Address?.Street ?? orderDetails.Delivery?.Address?.Street ?? orderDetails.Buyer?.Address?.Street,
            orderDetails.Invoice?.Address?.City ?? orderDetails.Delivery?.Address?.City ?? orderDetails.Buyer?.Address?.City, 
            orderDetails.Invoice?.Address?.ZipCode ?? orderDetails.Delivery?.Address?.ZipCode ?? orderDetails.Buyer?.Address?.ZipCode,
            orderDetails.Invoice?.Address?.CountryCode ?? orderDetails.Delivery?.Address?.CountryCode ?? orderDetails.Buyer?.Address?.CountryCode);

        var invoiceItems = orderDetails.LineItems.Select(x =>
                new InvoiceLineDto(x.Offer.Name,
                    decimal.Parse((string)x.Price.Amount, CultureInfo.InvariantCulture),
                    x.Quantity,
                    x.Price.Currency))
            .ToList();

        DateTime? dueDate = null;
        if (DateTime.TryParse(orderDetails.Invoice?.DueDate, out var parsed))
        {
            dueDate = parsed;
        }
        
        var paymentDetails = new PaymentTermsDto(
            orderDetails.Payment.FinishedAt ?? DateTime.UtcNow,
            dueDate ?? orderDetails.Payment.FinishedAt ?? DateTime.UtcNow,
            DateTime.UtcNow,
            orderDetails.Payment.Type == OrderPaymentType.Online);

        var deliveryDetails = new DeliveryDto(
             orderDetails.Delivery.Method?.Name ?? "KURIER",
            decimal.Parse(orderDetails.Delivery.Cost?.Amount ?? 0.ToString(),
                CultureInfo.InvariantCulture));
        
        return new GenerateInvoiceRequest(
            BuyerDto: buyerDetails,
            InvoiceLines: invoiceItems,
            PaymentTermsDto: paymentDetails,
            DeliveryDto: deliveryDetails,
            CorrelationKey: correlationKey,
            externalOrderNumber,
            tenantId,
            sourceSystemId);
    }
}