namespace AZ.Integrator.Procurement.Contracts.Suppliers;

public record CreateSupplierRequest(
    string Name,
    string TelephoneNumber,
    IEnumerable<SupplierMailboxRequest> Mailboxes
);

public record UpdateSupplierRequest(
    int Id,
    string Name,
    string TelephoneNumber,
    IEnumerable<SupplierMailboxRequest> Mailboxes
);

public record SupplierMailboxRequest(
    string Email
);
