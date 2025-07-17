using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Shared.ValueObjects;

public record OperatorCreationInformation
{
    public DateTime CreatedAt { get; }
    public string OperatorLogin { get; }
    public Guid OperatorId { get; }

    protected OperatorCreationInformation() { }
	
    public OperatorCreationInformation(DateTime creationDate, string createdByLogin, Guid createdById)
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        OperatorLogin = Guard.Against.OperatorCreationInformationCreator(createdByLogin);
        OperatorId = Guard.Against.CreationInformationCreator(createdById);
    }
        
    public static implicit operator DateTime(OperatorCreationInformation info)
        => info.CreatedAt;
        
    public static implicit operator OperatorCreationInformation(DateTime date)
        => new(date);
        
    public override string ToString() => CreatedAt.ToShortDateString();
}