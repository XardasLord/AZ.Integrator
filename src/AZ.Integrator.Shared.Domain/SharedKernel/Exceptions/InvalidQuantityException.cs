namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidQuantityException(int quantity, string message) : DomainException(message)
{
    public int Quantity { get; } = quantity;
    public override string Code => "invalid_quantity";
}