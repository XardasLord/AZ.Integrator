namespace AZ.Integrator.Domain.Aggregates.Invoice.ValueObjects;

public sealed record InvoiceNumber(string Value)
{
    public static implicit operator string(InvoiceNumber number)
        => number.Value;

    public static implicit operator InvoiceNumber(string number)
        => new(number);

    public override string ToString() => Value;
}