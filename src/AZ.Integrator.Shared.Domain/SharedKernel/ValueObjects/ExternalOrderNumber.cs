namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public sealed record ExternalOrderNumber(string Value)
{
    public static implicit operator string(ExternalOrderNumber number)
        => number.Value;

    public static implicit operator ExternalOrderNumber(string number)
        => new(number);

    public override string ToString() => Value;
}