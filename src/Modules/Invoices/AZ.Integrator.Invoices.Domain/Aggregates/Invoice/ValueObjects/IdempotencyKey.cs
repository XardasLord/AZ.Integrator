namespace AZ.Integrator.Invoices.Domain.Aggregates.Invoice.ValueObjects;

public sealed record IdempotencyKey(string Value)
{
    public static implicit operator string(IdempotencyKey number)
        => number.Value;

    public static implicit operator IdempotencyKey(string number)
        => new(number);

    public override string ToString() => Value;
}