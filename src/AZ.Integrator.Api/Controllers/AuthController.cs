﻿using AZ.Integrator.Infrastructure.ExternalServices.Allegro;
using AZ.Integrator.Infrastructure.Identity;
using AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AZ.Integrator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly UserDbContext _context;
    private readonly TokenService _tokenService;
    private readonly AllegroSettings _allegroSettings;

    public AuthController(UserManager<IdentityUser> userManager, UserDbContext context, TokenService tokenService, IOptions<AllegroSettings> allegroSettings)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
        _allegroSettings = allegroSettings.Value;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userManager.CreateAsync(
            new IdentityUser { UserName = request.Username, Email = request.Email},
            request.Password
        );
        
        if (result.Succeeded)
        {
            request.Password = "";
            return CreatedAtAction(nameof(Register), new {email = request.Email}, request);
        }
        
        foreach (var error in result.Errors) 
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        
        return BadRequest(ModelState);
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.FindByEmailAsync(request.Email);
        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }
        
        var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (userInDb is null)
        {
            return Unauthorized();
        }
        
        var accessToken = _tokenService.CreateToken(userInDb);
        
        await _context.SaveChangesAsync();
        
        return Ok(new AuthResponse
        {
            Username = userInDb.UserName,
            Email = userInDb.Email,
            Token = accessToken,
        });
    }

    [HttpGet("login-allegro")]
    public async Task<IActionResult> LoginViaAllegro()
    {
        return Challenge(new AuthenticationProperties
        {
           RedirectUri = $"http://localhost:8000/access_token={await HttpContext.GetTokenAsync("integrator_access_token")}"
        }, "allegro");
    }
}