using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;

public interface IInvoiceService
{
    Task<CreateInvoiceResponse> GenerateInvoice(
        BuyerDetails buyerDetails,
        List<LineItemDetails> lineItems,
        PaymentDetails paymentDetails,
        DeliveryDetails deliveryDetails);
    
    Task<byte[]> Download(long invoiceId);
}