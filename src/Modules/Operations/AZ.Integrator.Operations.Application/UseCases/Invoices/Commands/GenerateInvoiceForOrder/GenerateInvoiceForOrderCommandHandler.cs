using AZ.Integrator.Invoices.Contracts;
using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder.Strategy;
using Mediator;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;

public class GenerateInvoiceForOrderCommandHandler(
    IEnumerable<IInvoiceDraftBuilder> builders,
    IInvoicesFacade invoicesFacade)
    : IRequestHandler<GenerateInvoiceForOrderCommand, GenerateInvoiceResponse>
{
    public async ValueTask<GenerateInvoiceResponse> Handle(GenerateInvoiceForOrderCommand command, CancellationToken cancellationToken)
    {
        var builder = builders.FirstOrDefault(b => b.Provider == command.ShopProvider)
                      ?? throw new NotSupportedException($"Provider {command.ShopProvider} not supported");

        var request = await builder.BuildAsync(command.OrderNumber, command.TenantId, cancellationToken);

        var result = await invoicesFacade.GenerateInvoice(request, cancellationToken);

        return result;
    }
}