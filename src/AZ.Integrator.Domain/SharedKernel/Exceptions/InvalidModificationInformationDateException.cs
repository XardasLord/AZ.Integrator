namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidModificationInformationDateException : DomainException
{
    public DateTime CreationDate { get; }
    public override string Code => "invalid_modification_date";

    public InvalidModificationInformationDateException(DateTime creationDate, string message) : base(message)
    {
        CreationDate = creationDate;
    }
}