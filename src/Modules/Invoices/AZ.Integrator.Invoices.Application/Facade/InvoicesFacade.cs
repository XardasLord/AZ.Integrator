using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Invoices.Application.Common.Exceptions;
using AZ.Integrator.Invoices.Application.Common.ExternalServices.Fakturownia;
using AZ.Integrator.Invoices.Contracts;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.Specifications;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;
using AZ.Integrator.Monitoring.Contracts;

namespace AZ.Integrator.Invoices.Application.Facade;

public class InvoicesFacade(
    IInvoiceService invoiceService,
    IAggregateRepository<Invoice> invoiceRepository,
    IMonitoringFacade monitoringFacade,
    ICurrentUser currentUser,
    ICurrentDateTime currentDateTime) : IInvoicesFacade
{
    public async Task<GenerateInvoiceResponse> GenerateInvoice(GenerateInvoiceRequest request, CancellationToken cancellationToken)
    {
        // We could check if invoice already exists for given TenantId, ExternalOrderId and optionally IdempotencyKey
        
        var response = await invoiceService.GenerateInvoice(request.BuyerDto, request.InvoiceLines, request.PaymentTermsDto, request.DeliveryDto);
        
        if (response is null)
            throw new InvalidOperationException("Invoice generation failed");
        
        var invoice = Invoice.Generate(
            response.Id, response.Number, request.ExternalOrderId, InvoiceProvider.Fakturownia, 
            request.TenantId, request.SourceSystemId,
            currentUser, currentDateTime);

        invoice.SetIdempotencyKey(request.CorrelationKey);

        var events = invoice.Events.ToList();
        
        await invoiceRepository.AddAsync(invoice, cancellationToken);
        
        foreach (var @event in events)
        {
            await monitoringFacade.LogDomainEvent(
                @event, 
                invoice.CreationInformation.TenantId,
                invoice.CreationInformation.SourceSystemId,
                invoice.CreationInformation.CreatedBy,
                currentUser.UserName,
                invoice.CreationInformation.CreatedAt.DateTime,
                MonitoringSourceModuleEnum.Invoices.Name,
                invoice.ExternalId.Value.ToString(),
                invoice.Number.Value,
                invoice.IdempotencyKey!,
                cancellationToken);
        }

        return new GenerateInvoiceResponse
        {
            Id = response.Id,
            Number = response.Number,
        };
    }

    public async Task<GetInvoiceResponse> GetInvoice(GetInvoiceRequest request, CancellationToken cancellationToken)
    {
        var spec = new InvoiceByNumberSpec(request.InvoiceId, request.ExternalOrderId, 
            request.InvoiceProvider, request.TenantId, request.SourceSystemId);
        
        var invoice = await invoiceRepository.SingleOrDefaultAsync(spec, cancellationToken);
        
        if (invoice is null)
            throw new InvoiceNotFoundException($"Invoice for order '{request.ExternalOrderId}' was not found in database");
        
        var response = await invoiceService.Download(long.Parse(request.InvoiceId));

        return new GetInvoiceResponse
        {
            File = response,
            InvoiceNumber = invoice.Number
        };
    }
}