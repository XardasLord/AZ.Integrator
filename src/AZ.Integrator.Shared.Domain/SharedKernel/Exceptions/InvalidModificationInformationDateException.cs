namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidModificationInformationDateException : DomainException
{
    public DateTimeOffset CreationDate { get; }
    public override string Code => "invalid_modification_date";

    public InvalidModificationInformationDateException(DateTimeOffset creationDate, string message) : base(message)
    {
        CreationDate = creationDate;
    }
}