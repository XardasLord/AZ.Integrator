namespace AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;

public sealed record InvoiceExternalId(int Value)
{
    public static implicit operator int(InvoiceExternalId id)
        => id.Value;

    public static implicit operator InvoiceExternalId(int id)
        => new(id);

    public override string ToString() => Value.ToString();
}