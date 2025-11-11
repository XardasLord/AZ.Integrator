namespace AZ.Integrator.Platform.FeatureFlags.Abstractions;

public interface IFeatureFlags
{ 
    ValueTask<bool> IsEnabledAsync(string code, Guid tenantId, CancellationToken cancellationToken = default);
}