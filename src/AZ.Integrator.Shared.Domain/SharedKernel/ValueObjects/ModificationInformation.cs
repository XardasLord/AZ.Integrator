using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public record ModificationInformation
{
    public DateTimeOffset ModifiedAt { get; }
    public Guid ModifiedBy { get; }

    protected ModificationInformation() { }
	
    public ModificationInformation(DateTime creationDate, Guid createdBy)
    {
        ModifiedAt = Guard.Against.ModificationInformationDate(creationDate);
        ModifiedBy = Guard.Against.ModificationInformationCreator(createdBy);
    }
        
    public static implicit operator DateTimeOffset(ModificationInformation info)
        => info.ModifiedAt;
        
    public static implicit operator ModificationInformation(DateTimeOffset date)
        => new(date);
        
    public override string ToString() => ModifiedAt.Date.ToShortDateString();
}