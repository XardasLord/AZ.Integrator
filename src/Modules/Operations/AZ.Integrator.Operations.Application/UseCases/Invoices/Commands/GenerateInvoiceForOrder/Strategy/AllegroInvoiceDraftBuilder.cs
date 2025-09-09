using System.Globalization;
using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder.Strategy;

public sealed class AllegroInvoiceDraftBuilder(IAllegroService allegroService) : IInvoiceDraftBuilder
{
    public ShopProviderType Provider => ShopProviderType.Allegro;
    
    public async Task<GenerateInvoiceRequest> BuildAsync(string externalOrderNumber, string tenantId, CancellationToken cancellationToken)
    {
        var orderDetails = await allegroService.GetOrderDetails(Guid.Parse(externalOrderNumber), tenantId);
        
        var buyerDetails = new BuyerDto(
            orderDetails.Buyer.Email,
            orderDetails.Invoice?.Address?.NaturalPerson?.FirstName ?? orderDetails.Buyer.FirstName,
            orderDetails.Invoice?.Address?.NaturalPerson?.LastName ?? orderDetails.Buyer.LastName,
            orderDetails.Invoice?.Address?.Company?.Name,
            orderDetails.Invoice?.Address?.Company?.TaxId,
            orderDetails.Invoice?.Address?.Street ?? orderDetails.Buyer?.Address?.Street,
            orderDetails.Invoice?.Address?.City ?? orderDetails.Buyer?.Address?.City, 
            orderDetails.Invoice?.Address?.ZipCode ?? orderDetails.Buyer?.Address?.ZipCode,
            orderDetails.Invoice?.Address?.CountryCode ?? orderDetails.Buyer?.Address?.CountryCode);

        var invoiceItems = orderDetails.LineItems.Select(x =>
                new InvoiceLineDto(x.Offer.Name,
                    decimal.Parse((string)x.Price.Amount, CultureInfo.InvariantCulture),
                    x.Quantity,
                    x.Price.Currency))
            .ToList();

        DateTime? dueDate = null;
        if (DateTime.TryParse((string?)orderDetails.Invoice?.DueDate, out var parsed))
        {
            dueDate = parsed;
        }
        
        var paymentDetails = new PaymentTermsDto(
            orderDetails.Payment.FinishedAt ?? DateTime.UtcNow,
            dueDate ?? orderDetails.Payment.FinishedAt ?? DateTime.UtcNow,
            DateTime.UtcNow,
            orderDetails.Payment.Type == OrderPaymentType.Online);

        var deliveryDetails = new DeliveryDto(
            "KURIER",
            decimal.Parse(orderDetails.Delivery.Cost?.Amount ?? 0.ToString(),
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