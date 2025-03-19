using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;

namespace AZ.Integrator.Stocks.Domain.Aggregates.ValueObjects;

public record OperatorCreationInformation
{
    public DateTime CreatedAt { get; }
    public string OperatorId { get; }

    protected OperatorCreationInformation() { }
	
    public OperatorCreationInformation(DateTime creationDate, string createdBy)
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        OperatorId = Guard.Against.OperatorCreationInformationCreator(createdBy);
    }
        
    public static implicit operator DateTime(OperatorCreationInformation info)
        => info.CreatedAt;
        
    public static implicit operator OperatorCreationInformation(DateTime date)
        => new(date);
        
    public override string ToString() => CreatedAt.ToShortDateString();
}