using AZ.Integrator.Invoices.Contracts.Dtos;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;

public interface IInvoiceDraftBuilder
{
    Task<GenerateInvoiceRequest> BuildAsync(string externalOrderNumber, string tenantId, CancellationToken cancellationToken);
}