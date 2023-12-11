using MediatR;

namespace AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands.Register;

public record RegisterInvoiceCommand(string AllegroOrderNumber): IRequest;