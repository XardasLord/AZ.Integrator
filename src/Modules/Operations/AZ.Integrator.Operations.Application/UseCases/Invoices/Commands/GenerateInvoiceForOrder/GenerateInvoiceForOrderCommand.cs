using AZ.Integrator.Invoices.Contracts.Dtos;
using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;

public record GenerateInvoiceForOrderCommand(string OrderNumber, string CorrelationId = null) : HeaderRequest, IRequest<GenerateInvoiceResponse>;