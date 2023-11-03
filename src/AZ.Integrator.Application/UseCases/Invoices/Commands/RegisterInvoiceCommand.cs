using Mediator;

namespace AZ.Integrator.Application.UseCases.Invoices.Commands;

public record RegisterInvoiceCommand(string AllegroOrderNumber): ICommand;