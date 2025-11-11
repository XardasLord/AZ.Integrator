using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Platform.FeatureFlags.Infrastructure.Ef;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/me")]
public class MeController(FeatureFlagsDbContext dbContext, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet("feature-flags")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var data = await dbContext.TenantFeatureFlags
            .Where(x => x.TenantId == currentUser.TenantId)
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
}