using AZ.Integrator.Application.UseCases.Shipments.Commands.CreateDpdShipment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DpdShipmentsController : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> RegisterShipment(CreateDpdShipmentCommand command)
    {
        await Mediator.Send(command);
        
        return NoContent();
    }
}