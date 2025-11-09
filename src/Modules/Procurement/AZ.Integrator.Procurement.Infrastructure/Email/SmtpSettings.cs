namespace AZ.Integrator.Procurement.Infrastructure.Email;

public class SmtpSettings
{
    public const string SectionName = "Infrastructure:Smtp";
    
    public required string Host { get; init; }
    public int Port { get; init; }
    public required string From { get; init; }
    public required string FromName { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public bool UseSsl { get; init; } = true;
}

