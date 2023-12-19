using AZ.Integrator.TagParcelTemplates.Application.UseCases.Commands.SaveParcelTemplate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ParcelTemplatesController : ApiBaseController
{
    [HttpPut("{*tag}")]
    public async Task<IActionResult> RegisterShipment(string tag, [FromBody] SaveParcelTemplateCommand command)
    {
        command = command with
        {
            Tag = tag
        };
        
        await Mediator.Send(command);
        
        return NoContent();
    }
}