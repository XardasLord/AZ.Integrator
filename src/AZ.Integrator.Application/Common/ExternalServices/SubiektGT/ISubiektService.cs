using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Application.Common.ExternalServices.SubiektGT;

public interface ISubiektService
{
    Task<string> GenerateInvoice(string allegroOrderNumber, BuyerDetails buyerDetails, List<LineItemDetails> lineItems, SummaryDetails summary, PaymentDetails paymentDetails);
}