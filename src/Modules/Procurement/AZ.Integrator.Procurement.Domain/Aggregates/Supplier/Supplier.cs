using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Aggregates.Supplier;

public class Supplier : Entity<SupplierId>, IAggregateRoot
{
    private SupplierName _name;
    private TenantId _tenantId;
    private TelephoneNumber _telephoneNumber;
    private TenantCreationInformation _creationInformation;
    private ModificationInformation _modificationInformation;
    
    private List<SupplierMailbox> _mailboxes;

    public SupplierName Name => _name;
    public TenantId TenantId => _tenantId;
    public TelephoneNumber TelephoneNumber => _telephoneNumber;
    public TenantCreationInformation CreationInformation => _creationInformation;
    public ModificationInformation ModificationInformation => _modificationInformation;
    
    public IReadOnlyCollection<SupplierMailbox> Mailboxes => _mailboxes.AsReadOnly();

    private Supplier()
    {
        _mailboxes = [];
    }

    public static Supplier Create(
        SupplierName name, TelephoneNumber telephoneNumber, List<Email> mailboxEmails,
        ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        return new Supplier
        {
            _name = name,
            _tenantId = currentUser.TenantId,
            _telephoneNumber = telephoneNumber,
            _mailboxes = mailboxEmails.Select(email => new SupplierMailbox(email)).ToList(),
            _creationInformation = new TenantCreationInformation(
                currentDateTime.CurrentDate(),
                currentUser.UserId, 
                currentUser.TenantId),
            _modificationInformation = new ModificationInformation(currentDateTime.CurrentDate(), currentUser.UserId),
        };
    }

    public void Update(
        SupplierName name, TelephoneNumber telephoneNumber, List<Email> mailboxEmails,
        ICurrentUser currentUser, ICurrentDateTime currentDateTime)
    {
        _name = name;
        _telephoneNumber = telephoneNumber;
        _modificationInformation = new ModificationInformation(currentDateTime.CurrentDate(), currentUser.UserId);
        
        AssignMailboxes(mailboxEmails);
    }

    private void AssignMailboxes(List<Email> mailboxEmails)
    {
        if (_mailboxes.Count > 1)
            _mailboxes.Clear();
        
        _mailboxes = mailboxEmails.Select(email => new SupplierMailbox(email)).ToList();
    }
}