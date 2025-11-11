using AZ.Integrator.Platform.FeatureFlags.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AZ.Integrator.Platform.FeatureFlags.Infrastructure;

public sealed class FeatureFlagsService(FeatureFlagsDbContext db, IMemoryCache cache) : IFeatureFlags
{
    private const string Prefix = "ff";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    public async ValueTask<bool> IsEnabledAsync(string code, Guid tenantId, CancellationToken ct = default)
    {
        var key = $"{Prefix}:{tenantId}:{code}";
        if (cache.TryGetValue<bool>(key, out var cached))
            return cached;

        var enabled = await db.TenantFeatureFlags
            .AsNoTracking()
            .Where(f => f.TenantId == tenantId && f.Code == code)
            .Select(f => f.Enabled)
            .FirstOrDefaultAsync(ct);

        cache.Set(key, enabled, CacheTtl);
        
        return enabled;
    }

    public void Invalidate(Guid tenantId, string code)
    {
        cache.Remove($"{Prefix}:{tenantId}:{code}");
    }
}