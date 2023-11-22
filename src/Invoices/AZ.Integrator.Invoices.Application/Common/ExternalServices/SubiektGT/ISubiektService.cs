using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Invoices.Application.Common.ExternalServices.SubiektGT;

public interface ISubiektService
{
    Task<string> GenerateInvoice(
        string allegroOrderNumber,
        BuyerDetails buyerDetails,
        List<LineItemDetails> lineItems,
        SummaryDetails summary,
        PaymentDetails paymentDetails,
        DeliveryDetails deliveryDetails);
}