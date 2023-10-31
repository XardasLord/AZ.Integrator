using AZ.Integrator.Application.Common.ExternalServices.SubiektGT;
using AZ.Integrator.Domain.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

public class TestController : ApiBaseController
{
    private readonly ICurrentUser _currentUser;
    private readonly ISubiektService _subiektService;

    public TestController(ICurrentUser currentUser, ISubiektService subiektService)
    {
        _currentUser = currentUser;
        _subiektService = subiektService;
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_currentUser.AllegroAccessToken);
    }
    
    [HttpGet("subiekt")]
    public async Task<IActionResult> OpenSubiekt()
    {
        var test = await _subiektService.GenerateSaleDocument();

        return Ok(test);
    }
}