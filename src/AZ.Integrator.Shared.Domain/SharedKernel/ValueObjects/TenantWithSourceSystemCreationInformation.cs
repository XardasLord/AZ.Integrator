using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public record TenantWithSourceSystemCreationInformation
{
    public DateTimeOffset CreatedAt { get; }
    public Guid CreatedBy { get; }
    public TenantId TenantId { get; }
    public SourceSystemId SourceSystemId { get; }
    

    protected TenantWithSourceSystemCreationInformation() { }
	
    public TenantWithSourceSystemCreationInformation(DateTimeOffset creationDate, Guid createdBy, Guid tenantId, string sourceSystemId)
        : this()
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        CreatedBy = Guard.Against.CreationInformationCreator(createdBy);
        TenantId = tenantId;
        SourceSystemId = sourceSystemId;
    }
        
    public static implicit operator DateTimeOffset(TenantWithSourceSystemCreationInformation info)
        => info.CreatedAt;
        
    public static implicit operator TenantWithSourceSystemCreationInformation(DateTimeOffset date)
        => new(date);
        
    public override string ToString() => CreatedAt.Date.ToShortDateString();
}