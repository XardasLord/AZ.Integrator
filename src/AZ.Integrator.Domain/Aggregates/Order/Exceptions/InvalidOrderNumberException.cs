using AZ.Integrator.Domain.SharedKernel.Exceptions;

namespace AZ.Integrator.Domain.Aggregates.Order.Exceptions;

public class InvalidOrderNumberException : DomainException
{
    public string OrderNumber { get; }
    public override string Code => "invalid_order_number";

    public InvalidOrderNumberException(string orderNumber, string message) : base(message)
    {
        OrderNumber = orderNumber;
    }
}