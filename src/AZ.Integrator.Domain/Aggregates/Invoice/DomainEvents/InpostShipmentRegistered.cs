using AZ.Integrator.Domain.Aggregates.Invoice.ValueObjects;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Mediator;

namespace AZ.Integrator.Domain.Aggregates.Invoice.DomainEvents;

public record InvoiceCreated(InvoiceNumber InvoiceNumber, AllegroOrderNumber AllegroOrderNumber) : INotification;