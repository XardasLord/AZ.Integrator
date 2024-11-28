using Mediator;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands.Register;

public record RegisterInvoiceCommand(string OrderNumber): IRequest;