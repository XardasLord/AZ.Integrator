using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.Aggregates.Order.ValueObjects;

public sealed record OrderNumber
{
    public string Value { get; }

    private OrderNumber() { }

    public OrderNumber(string number)
    {
        Value = Guard.Against.OrderNumber(number, nameof(OrderNumber));
    }

    public static implicit operator string(OrderNumber number)
        => number.Value;

    public static implicit operator OrderNumber(string number)
        => new(number);

    public override string ToString() => Value;
}