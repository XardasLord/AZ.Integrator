using Ardalis.GuardClauses;
using AZ.Integrator.Domain.Extensions;

namespace AZ.Integrator.Domain.SharedKernel.ValueObjects;

public sealed record Email
{
    public static readonly Email Empty = new();
    
    public string Value { get; }
    
    private Email() { }
	
    public Email(string email)
    {
        Value = Guard.Against.Email(email, nameof(Email));
    }
        
    public static implicit operator string(Email email)
        => email.Value;
        
    public static implicit operator Email(string email)
        => new(email);
        
    public override string ToString() => Value;
}