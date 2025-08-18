using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public record CreationInformation
{
    public DateTimeOffset CreatedAt { get; }
    public Guid CreatedBy { get; }

    protected CreationInformation() { }
	
    public CreationInformation(DateTime creationDate, Guid createdBy)
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        CreatedBy = Guard.Against.CreationInformationCreator(createdBy);
    }
        
    public static implicit operator DateTimeOffset(CreationInformation info)
        => info.CreatedAt;
        
    public static implicit operator CreationInformation(DateTimeOffset date)
        => new(date);
        
    public override string ToString() => CreatedAt.Date.ToShortDateString();
}