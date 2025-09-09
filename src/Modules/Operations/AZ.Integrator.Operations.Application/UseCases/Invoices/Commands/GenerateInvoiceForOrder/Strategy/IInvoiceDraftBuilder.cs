using AZ.Integrator.Domain.SharedKernel;
using AZ.Integrator.Invoices.Contracts.Dtos;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder.Strategy;

public interface IInvoiceDraftBuilder
{
    ShopProviderType Provider { get; }
    Task<GenerateInvoiceRequest> BuildAsync(string externalOrderNumber, string tenantId, CancellationToken cancellationToken);
}