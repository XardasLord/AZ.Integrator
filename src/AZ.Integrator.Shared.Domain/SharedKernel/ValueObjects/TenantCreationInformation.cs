using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public record TenantCreationInformation
{
    public DateTimeOffset CreatedAt { get; }
    public Guid CreatedBy { get; }
    public TenantId TenantId { get; }
    

    protected TenantCreationInformation() { }
	
    public TenantCreationInformation(DateTimeOffset creationDate, Guid createdBy, Guid tenantId)
        : this()
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        CreatedBy = Guard.Against.CreationInformationCreator(createdBy);
        TenantId = tenantId;
    }
        
    public static implicit operator DateTimeOffset(TenantCreationInformation info)
        => info.CreatedAt;
        
    public static implicit operator TenantCreationInformation(DateTimeOffset date)
        => new(date);
        
    public override string ToString() => CreatedAt.Date.ToShortDateString();
}