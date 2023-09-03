namespace AZ.Integrator.Domain.SharedKernel.Exceptions;

public class InvalidDeletionInformationDateException : DomainException
{
    public DateTime DeletionDate { get; }
    public override string Code => "invalid_deletion_date";

    public InvalidDeletionInformationDateException(DateTime deletionnDate, string message) : base(message)
    {
        DeletionDate = deletionnDate;
    }
}