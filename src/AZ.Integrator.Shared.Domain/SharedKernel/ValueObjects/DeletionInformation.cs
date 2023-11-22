using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public sealed record DeletionInformation
{
    public DateTime DeletedAt { get; }
    public Guid DeletedBy { get; }

    private DeletionInformation() { }
	
    public DeletionInformation(DateTime deletionDate, Guid deletedBy)
    {
        DeletedAt = Guard.Against.DeletionInformationDate(deletionDate);
        DeletedBy = Guard.Against.DeletionInformationPerson(deletedBy);
    }
        
    public static implicit operator DateTime(DeletionInformation email)
        => email.DeletedAt;
        
    public static implicit operator DeletionInformation(DateTime email)
        => new(email);
        
    public override string ToString() => DeletedAt.ToShortDateString();
}