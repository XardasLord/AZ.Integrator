namespace AZ.Integrator.Procurement.Contracts.Suppliers;

public class SupplierViewModel
{
    public int Id { get; init; }
    public Guid TenantId { get; init; }
    public required string Name { get; init; }
    public string TelephoneNumber { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid ModifiedBy { get; init; }
    public DateTime ModifiedAt { get; init; }
    public required IReadOnlyCollection<SupplierMailboxViewModel> Mailboxes { get; init; }
};

public class SupplierMailboxViewModel
{
    public int Id { get; init; }
    public int SupplierId { get; init; }
    public required string Email { get; init; }
};
