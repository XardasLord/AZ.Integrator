using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.Aggregates.Invoice.DomainEvents;
using AZ.Integrator.Domain.Aggregates.Invoice.ValueObjects;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Domain.Aggregates.Invoice;

public class Invoice : Entity, IAggregateRoot
{
    private InvoiceNumber _number;
    private AllegroOrderNumber _allegroAllegroOrderNumber;
    private CreationInformation _creationInformation;
    
    public InvoiceNumber Number => _number;
    public AllegroOrderNumber AllegroAllegroOrderNumber => _allegroAllegroOrderNumber;
    public CreationInformation CreationInformation => _creationInformation;
    
    private Invoice() { }

    private Invoice(InvoiceNumber number, AllegroOrderNumber allegroAllegroOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _number = number;
        _allegroAllegroOrderNumber = allegroAllegroOrderNumber;
        _creationInformation = new CreationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
    }

    public static Invoice Create(InvoiceNumber number, AllegroOrderNumber allegroAllegroOrderNumber, ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var invoice = new Invoice(number, allegroAllegroOrderNumber, currentUser, currentDateTime);
        
        invoice.AddDomainEvent(new InvoiceCreated(number, allegroAllegroOrderNumber));
        
        return invoice;
    }
}