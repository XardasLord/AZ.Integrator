using AZ.Integrator.Application.UseCases.Shipments.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InpostShipmentsController : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> RegisterShipment(CreateInpostShipmentCommand command)
    {
        await Mediator.Send(command);
        
        return NoContent();
    }
}