using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Infrastructure;
using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Ef;
using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Api.Controllers;

[ApiController]
[Route("admin/tenants/{tenantId:guid}/feature-flags")]
public class FeatureFlagsController(
    FeatureFlagsDbContext dbContext,
    FeatureFlagsService featureFlagsService,
    ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(Guid tenantId, CancellationToken ct)
    {
        var data = await dbContext.TenantFeatureFlags
            .Where(x => x.TenantId == tenantId)
            .Select(x => new
            {
                x.Code,
                x.Enabled,
                x.ModifiedAt,
                x.ModifiedBy
            })
            .ToListAsync(ct);
        
        return Ok(data);
    }

    public record UpsertFlagDto(bool Enabled);

    [HttpPut("{code}")]
    public async Task<IActionResult> Upsert(Guid tenantId, string code, [FromBody] UpsertFlagDto dto, CancellationToken cancellationToken)
    {
        var flagExists = await dbContext.FeatureFlags.AnyAsync(f => f.Code == code, cancellationToken);
        if (!flagExists)
        {
            dbContext.FeatureFlags.Add(new FeatureFlag
            {
                Code = code, 
                Description = code
            });
        }

        var tenantFeatureFlag = await dbContext.TenantFeatureFlags.FindAsync([tenantId, code], cancellationToken);
        if (tenantFeatureFlag is null)
        {
            tenantFeatureFlag = new TenantFeatureFlag
            {
                TenantId = tenantId,
                Code = code,
                Enabled = dto.Enabled, 
                ModifiedAt = DateTimeOffset.UtcNow,
                ModifiedBy = currentUser.UserId
            };
            dbContext.TenantFeatureFlags.Add(tenantFeatureFlag);
        }
        else
        {
            tenantFeatureFlag.Enabled = dto.Enabled;
            tenantFeatureFlag.ModifiedAt = DateTimeOffset.UtcNow;
            tenantFeatureFlag.ModifiedBy = currentUser.UserId;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        
        featureFlagsService.Invalidate(tenantId, code);
        
        return NoContent();
    }
}