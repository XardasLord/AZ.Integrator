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
    private AllegroOrderNumber _allegroAllegroOrderNumber;
    private CreationInformation _creationInformation;

    public InvoiceExternalId ExternalId => _externalId;
    public InvoiceNumber Number => _number;
    public AllegroOrderNumber AllegroAllegroOrderNumber => _allegroAllegroOrderNumber;
    public CreationInformation CreationInformation => _creationInformation;
    
    private Invoice() { }

    private Invoice(InvoiceExternalId externalId, InvoiceNumber number, AllegroOrderNumber allegroAllegroOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _externalId = externalId;
        _number = number;
        _allegroAllegroOrderNumber = allegroAllegroOrderNumber;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
    }

    public static Invoice Create(InvoiceExternalId externalId, InvoiceNumber number, AllegroOrderNumber allegroAllegroOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var invoice = new Invoice(externalId, number, allegroAllegroOrderNumber, currentUser, currentDateTime);
        
        invoice.AddDomainEvent(new InvoiceCreated(externalId, number, allegroAllegroOrderNumber));
        
        return invoice;
    }
}