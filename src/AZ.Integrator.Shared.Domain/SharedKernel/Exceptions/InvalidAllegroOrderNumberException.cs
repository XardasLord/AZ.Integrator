namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidAllegroOrderNumberException : DomainException
{
    public string AllegroOrderNumber { get; }
    public override string Code => "invalid_allegro_order_number";

    public InvalidAllegroOrderNumberException(string allegroOrderNumber, string message) : base(message)
    {
        AllegroOrderNumber = allegroOrderNumber;
    }
}