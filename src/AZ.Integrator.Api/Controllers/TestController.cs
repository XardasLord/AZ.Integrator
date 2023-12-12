using AZ.Integrator.Domain.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

public class TestController : ApiBaseController
{
    private readonly ICurrentUser _currentUser;

    public TestController(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("tenant")]
    public IActionResult GetTenantId()
    {
        return Ok(_currentUser.TenantId);
    }
}