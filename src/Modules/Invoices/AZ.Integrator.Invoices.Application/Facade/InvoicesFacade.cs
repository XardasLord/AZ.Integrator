using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Application.Common.Exceptions;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Contracts;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.Specifications;
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
        
        var invoice = Invoice.Generate(response.Id, response.Number, request.ExternalOrderId,
            InvoiceProvider.Fakturownia, request.TenantId, currentUser, currentDateTime);

        invoice.SetIdempotencyKey(request.IdempotencyKey);
        
        await invoiceRepository.AddAsync(invoice, cancellationToken);

        return new GenerateInvoiceResponse
        {
            Id = response.Id,
            Number = response.Number,
        };
    }

    public async Task<GetInvoiceResponse> GetInvoice(GetInvoiceRequest request, CancellationToken cancellationToken)
    {
        var spec = new InvoiceByNumberSpec(request.InvoiceId, request.ExternalOrderId, 
            request.InvoiceProvider, request.TenantId);
        
        var invoice = await invoiceRepository.SingleOrDefaultAsync(spec, cancellationToken);
        
        if (invoice is null)
            throw new InvoiceNotFoundException($"Invoice for order '{request.ExternalOrderId}' was not found in database");
        
        var response = await invoiceService.Download(long.Parse(request.InvoiceId));

        return new GetInvoiceResponse()
        {
            File = response,
            InvoiceNumber = invoice.Number
        };
    }
}