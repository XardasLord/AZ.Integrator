using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public record TenantCreationInformation
{
    public DateTime CreatedAt { get; }
    public Guid CreatedBy { get; }
    public TenantId TenantId { get; }
    

    protected TenantCreationInformation() { }
	
    public TenantCreationInformation(DateTime creationDate, Guid createdBy, string tenantId)
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        CreatedBy = Guard.Against.CreationInformationCreator(createdBy);
        TenantId = tenantId;
    }
        
    public static implicit operator DateTime(TenantCreationInformation info)
        => info.CreatedAt;
        
    public static implicit operator TenantCreationInformation(DateTime date)
        => new(date);
        
    public override string ToString() => CreatedAt.ToShortDateString();
}