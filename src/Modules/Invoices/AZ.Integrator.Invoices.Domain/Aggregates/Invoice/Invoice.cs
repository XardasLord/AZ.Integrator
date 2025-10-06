using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;
using AZ.Integrator.Invoices.Domain.Events;

namespace AZ.Integrator.Invoices.Domain.Aggregates.Invoice;

public class Invoice : Entity, IAggregateRoot
{
    private InvoiceExternalId _externalId;
    private InvoiceNumber _number;
    private ExternalOrderNumber _externalOrderNumber;
    private IdempotencyKey _idempotencyKey;
    private InvoiceProvider _invoiceProvider;
    private TenantCreationInformation _creationInformation;

    public InvoiceExternalId ExternalId => _externalId;
    public InvoiceNumber Number => _number;
    public ExternalOrderNumber ExternalOrderNumber => _externalOrderNumber;
    public IdempotencyKey IdempotencyKey => _idempotencyKey;
    public InvoiceProvider InvoiceProvider => _invoiceProvider;
    public TenantCreationInformation CreationInformation => _creationInformation;
    
    private Invoice() { }

    private Invoice(
        InvoiceExternalId externalId, InvoiceNumber number, ExternalOrderNumber externalOrderNumber, 
        InvoiceProvider invoiceProvider, TenantId tenantId, SourceSystemId sourceSystemId,
        ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _externalId = externalId;
        _number = number;
        _externalOrderNumber = externalOrderNumber;
        _invoiceProvider = invoiceProvider;
        _creationInformation = new TenantCreationInformation(currentDateTime.CurrentDate(), currentUser.UserId, tenantId, sourceSystemId);
    }

    public static Invoice Generate(
        InvoiceExternalId externalId, InvoiceNumber number, ExternalOrderNumber externalOrderNumber, 
        InvoiceProvider invoiceProvider, TenantId tenantId, SourceSystemId sourceSystemId, 
        ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        var invoice = new Invoice(
            externalId, number, externalOrderNumber, 
            invoiceProvider, tenantId, sourceSystemId,
            currentUser, currentDateTime);
        
        invoice.AddDomainEvent(new InvoiceGenerated(externalId.ToString(), number, externalOrderNumber, (int)invoiceProvider, tenantId));
        
        return invoice;
    }

    public void SetIdempotencyKey(string idempotencyKey)
    {
        _idempotencyKey = idempotencyKey;
    }
}