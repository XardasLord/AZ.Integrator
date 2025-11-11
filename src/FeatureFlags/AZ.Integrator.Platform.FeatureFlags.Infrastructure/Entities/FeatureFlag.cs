namespace AZ.Integrator.Platform.FeatureFlags.Infrastructure.Entities;

public class FeatureFlag
{
    public string Code { get; set; } = default!;
    public string Description { get; set; } = "";
}