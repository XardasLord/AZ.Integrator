namespace AZ.Integrator.Shared.Infrastructure.Authentication;

public class IdentityOptions
{
    public string PrivateKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiresInHours { get; set; }
}