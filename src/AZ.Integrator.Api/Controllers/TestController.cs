using AZ.Integrator.Domain.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

public class TestController(ICurrentUser currentUser) : ApiBaseController
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("tenant")]
    public IActionResult GetTenantId()
    {
        return Ok(currentUser.UserName);
    }
}