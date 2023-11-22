namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public sealed record AllegroOrderNumber(string Value)
{
    public static implicit operator string(AllegroOrderNumber number)
        => number.Value;

    public static implicit operator AllegroOrderNumber(string number)
        => new(number);

    public override string ToString() => Value;
}