using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public record CreationInformation
{
    public DateTimeOffset CreatedAt { get; }
    public Guid CreatedBy { get; }
    public TenantId TenantId { get; }

    protected CreationInformation() { }
	
    public CreationInformation(DateTime creationDate, Guid createdBy, TenantId tenantId)
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        CreatedBy = Guard.Against.CreationInformationCreator(createdBy);
        TenantId = Guard.Against.TenantId(tenantId);
    }
        
    public static implicit operator DateTimeOffset(CreationInformation info)
        => info.CreatedAt;
        
    public static implicit operator CreationInformation(DateTimeOffset date)
        => new(date);
        
    public override string ToString() => CreatedAt.Date.ToShortDateString();
}