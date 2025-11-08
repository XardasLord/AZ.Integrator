namespace AZ.Integrator.Procurement.Application.Common.Settings;

public class ApplicationSettings
{
    public const string SectionName = "Application:Procurement";
    
    public required string CustomerEmail { get; init; }
    public bool IncludeCustomerEmailInCc { get; init; }
}