namespace AZ.Integrator.Invoices.Contracts.Dtos;

public sealed record GenerateInvoiceRequest(
    BuyerDto BuyerDto,
    IReadOnlyList<InvoiceLineDto> InvoiceLines,
    PaymentTermsDto PaymentTermsDto,
    DeliveryDto DeliveryDto,
    string CorrelationKey,
    string ExternalOrderId,
    Guid TenantId,
    string SourceSystemId);
    
public sealed record BuyerDto(
    string Email,
    string FirstName,
    string LastName,
    string CompanyName,
    string TaxNo,
    string Street,
    string City,
    string PostCode,
    string Country);

public sealed record InvoiceLineDto(string ItemName, decimal Amount, int Quantity, string Currency);

public sealed record PaymentTermsDto(DateTime SellDate, DateTime PaymentToDate, DateTime IssueDate, bool IsPaid);

public sealed record DeliveryDto(string DeliveryItemName, decimal Amount, int Quantity = 1);