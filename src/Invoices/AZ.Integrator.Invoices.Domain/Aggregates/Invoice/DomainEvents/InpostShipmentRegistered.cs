using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;
using MediatR;

namespace AZ.Integrator.Invoices.Domain.Aggregates.Invoice.DomainEvents;

public record InvoiceCreated(InvoiceNumber InvoiceNumber, AllegroOrderNumber AllegroOrderNumber) : INotification;