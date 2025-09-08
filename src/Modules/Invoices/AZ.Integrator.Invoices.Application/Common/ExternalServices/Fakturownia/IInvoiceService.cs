using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;

namespace AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;

public interface IInvoiceService
{
    Task<CreateInvoiceResponse> GenerateInvoice(
        BuyerDetails buyerDetails,
        List<InvoiceItem> invoiceItems,
        PaymentDetails paymentDetails,
        DeliveryDetails deliveryDetails);
    
    Task<byte[]> Download(long invoiceId);
}

public record BuyerDetails(
    string Email,
    string FirstName,
    string LastName,
    string CompanyName,
    string PersonalIdentity,
    string PhoneNumber);

public record InvoiceItem(string ItemName, decimal Amount, int Quantity, string Currency);

public record PaymentDetails(DateTime SellDate, DateTime PaymentToDate, DateTime IssueDate);

public record DeliveryDetails(string DeliveryItemName, decimal Amount, int Quantity = 1);