using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Stocks.Domain.Aggregates.Shared.ValueObjects;

public record OperatorCreationInformation
{
    public DateTimeOffset CreatedAt { get; }
    public string OperatorLogin { get; }
    public Guid OperatorId { get; }

    protected OperatorCreationInformation() { }
	
    public OperatorCreationInformation(DateTime creationDate, string createdByLogin, Guid createdById)
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        OperatorLogin = Guard.Against.OperatorCreationInformationCreator(createdByLogin);
        OperatorId = Guard.Against.CreationInformationCreator(createdById);
    }
        
    public static implicit operator DateTimeOffset(OperatorCreationInformation info)
        => info.CreatedAt;
        
    public static implicit operator OperatorCreationInformation(DateTimeOffset date)
        => new(date);
        
    public override string ToString() => CreatedAt.Date.ToShortDateString();
}