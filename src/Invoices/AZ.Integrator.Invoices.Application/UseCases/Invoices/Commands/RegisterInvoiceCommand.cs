using MediatR;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands;

public record RegisterInvoiceCommand(string AllegroOrderNumber): IRequest;