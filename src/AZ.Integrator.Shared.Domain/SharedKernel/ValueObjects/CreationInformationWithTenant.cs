﻿using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public record CreationInformationWithTenant
{
    public DateTime CreatedAt { get; }
    public Guid CreatedBy { get; }
    public TenantId TenantId { get; }
    

    protected CreationInformationWithTenant() { }
	
    public CreationInformationWithTenant(DateTime creationDate, Guid createdBy, string tenantId)
    {
        CreatedAt = Guard.Against.CreationInformationDate(creationDate);
        CreatedBy = Guard.Against.CreationInformationCreator(createdBy);
        TenantId = tenantId;
    }
        
    public static implicit operator DateTime(CreationInformationWithTenant info)
        => info.CreatedAt;
        
    public static implicit operator CreationInformationWithTenant(DateTime date)
        => new(date);
        
    public override string ToString() => CreatedAt.ToShortDateString();
}