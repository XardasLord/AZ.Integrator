using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public record ModificationInformation
{
    public DateTime ModifiedAt { get; }
    public Guid ModifiedBy { get; }

    protected ModificationInformation() { }
	
    public ModificationInformation(DateTime creationDate, Guid createdBy)
    {
        ModifiedAt = Guard.Against.ModificationInformationDate(creationDate);
        ModifiedBy = Guard.Against.ModificationInformationCreator(createdBy);
    }
        
    public static implicit operator DateTime(ModificationInformation info)
        => info.ModifiedAt;
        
    public static implicit operator ModificationInformation(DateTime date)
        => new(date);
        
    public override string ToString() => ModifiedAt.ToShortDateString();
}