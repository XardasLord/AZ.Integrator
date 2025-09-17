using AZ.Integrator.Invoices.Contracts.Dtos;

namespace AZ.Integrator.Invoices.Contracts;

public interface IInvoicesFacade
{
    Task<GenerateInvoiceResponse> GenerateInvoice(GenerateInvoiceRequest request, CancellationToken cancellationToken);
    Task<GetInvoiceResponse> GetInvoice(GetInvoiceRequest request, CancellationToken cancellationToken);
}