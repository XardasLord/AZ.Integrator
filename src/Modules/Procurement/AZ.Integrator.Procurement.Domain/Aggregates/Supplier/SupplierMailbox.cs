using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Aggregates.Supplier;

public class SupplierMailbox : Entity<int>
{
    private Email _email;

    public Email Email => _email;

    private SupplierMailbox()
    {
    }

    internal SupplierMailbox(Email email) 
        : this()
    {
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    internal void UpdateEmail(Email email)
    {
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }
}