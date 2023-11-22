namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidDeletionInformationPersonException : DomainException
{
    public Guid User { get; }
    public override string Code => "invalid_person";

    public InvalidDeletionInformationPersonException(Guid user, string message) : base(message)
    {
        User = user;
    }
}