using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Contracts;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;

namespace AZ.Integrator.Invoices.Application.Facade;

public class InvoicesFacade(
    IInvoiceService invoiceService,
    IAggregateRepository<Invoice> invoiceRepository,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime) : IInvoicesFacade
{
    public async Task<GenerateInvoiceResponse> GenerateInvoice(GenerateInvoiceRequest request, CancellationToken cancellationToken)
    {
        // We could check if invoice already exists for given TenantId, ExternalOrderId and optionally IdempotencyKey
        
        var response = await invoiceService.GenerateInvoice(request.BuyerDto, request.InvoiceLines, request.PaymentTermsDto, request.DeliveryDto);
        
        if (response is null)
            throw new InvalidOperationException("Invoice generation failed");
        
        var invoice = Invoice.Create(response.Id, response.Number, request.ExternalOrderId,
            InvoiceProvider.Fakturownia, request.TenantId, currentUser, currentDateTime);
        
        await invoiceRepository.AddAsync(invoice, cancellationToken);

        return new GenerateInvoiceResponse
        {
            Id = response.Id,
            Number = response.Number,
        };
    }
}