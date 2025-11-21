using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia.Models;
using AZ.Integrator.Invoices.Contracts.Dtos;

namespace AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;

public interface IInvoiceService
{
    Task<CreateInvoiceResponse> GenerateInvoice(
        BuyerDto buyerDto,
        IReadOnlyList<InvoiceLineDto> invoiceItems,
        PaymentTermsDto paymentTermsDto,
        DeliveryDto deliveryDto,
        TenantId tenantId);
    
    Task<byte[]> Download(long invoiceId, TenantId tenantId);
}