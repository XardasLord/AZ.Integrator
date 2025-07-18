namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidCreationInformationDateException : DomainException
{
    public DateTimeOffset CreationDate { get; }
    public override string Code => "invalid_creation_date";

    public InvalidCreationInformationDateException(DateTimeOffset creationDate, string message) : base(message)
    {
        CreationDate = creationDate;
    }
}