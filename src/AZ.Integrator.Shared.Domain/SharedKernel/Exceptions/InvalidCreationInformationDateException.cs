namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidCreationInformationDateException : DomainException
{
    public DateTime CreationDate { get; }
    public override string Code => "invalid_creation_date";

    public InvalidCreationInformationDateException(DateTime creationDate, string message) : base(message)
    {
        CreationDate = creationDate;
    }
}