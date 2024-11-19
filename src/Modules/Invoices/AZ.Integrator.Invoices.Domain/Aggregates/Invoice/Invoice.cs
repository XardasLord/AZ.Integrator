using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.DomainEvents;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;

namespace AZ.Integrator.Invoices.Domain.Aggregates.Invoice;

public class Invoice : Entity, IAggregateRoot
{
    private InvoiceExternalId _externalId;
    private InvoiceNumber _number;
    private ExternalOrderNumber _externalOrderNumber;
    private CreationInformation _creationInformation;

    public InvoiceExternalId ExternalId => _externalId;
    public InvoiceNumber Number => _number;
    public ExternalOrderNumber ExternalOrderNumber => _externalOrderNumber;
    public CreationInformation CreationInformation => _creationInformation;
    
    private Invoice() { }

    private Invoice(InvoiceExternalId externalId, InvoiceNumber number, ExternalOrderNumber externalOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _externalId = externalId;
        _number = number;
        _externalOrderNumber = externalOrderNumber;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
    }

    public static Invoice Create(InvoiceExternalId externalId, InvoiceNumber number, ExternalOrderNumber externalExternalOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var invoice = new Invoice(externalId, number, externalExternalOrderNumber, currentUser, currentDateTime);
        
        invoice.AddDomainEvent(new InvoiceCreated(externalId, number, externalExternalOrderNumber));
        
        return invoice;
    }
}