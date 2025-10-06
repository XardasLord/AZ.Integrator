using AZ.Integrator.Invoices.Contracts;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Orders.Contracts;
using Mediator;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;

public class GenerateInvoiceForOrderCommandHandler(
    IInvoiceDraftBuilder invoiceDraftBuilder,
    IInvoicesFacade invoicesFacade)
    : IRequestHandler<GenerateInvoiceForOrderCommand, GenerateInvoiceResponse>
{
    public async ValueTask<GenerateInvoiceResponse> Handle(GenerateInvoiceForOrderCommand command, CancellationToken cancellationToken)
    {
        var request = await invoiceDraftBuilder.BuildAsync(command.OrderNumber, command.TenantId, command.SourceSystemId, cancellationToken);

        var result = await invoicesFacade.GenerateInvoice(request, cancellationToken);

        return result;
    }
}