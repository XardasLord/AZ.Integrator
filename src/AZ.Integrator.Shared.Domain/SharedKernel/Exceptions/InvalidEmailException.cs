namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidEmailException : DomainException
{
    public string Email { get; }
    public override string Code => "invalid_email";

    public InvalidEmailException(string email, string message) : base(message)
    {
        Email = email;
    }
}