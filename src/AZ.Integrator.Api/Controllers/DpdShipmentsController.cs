using AZ.Integrator.Shipments.Application.UseCases.Shipments.Commands.CreateDpdShipment;
using AZ.Integrator.Shipments.Application.UseCases.Shipments.Queries.GetDpdLabel;
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
    
    [HttpGet("{sessionId}/label")]
    public async Task<IActionResult> GetShipmentLabel(long sessionId)
    {
        var result = await Mediator.Send(new GetDpdLabelQuery(sessionId));

        return File(result.ContentStream, result.ContentType, result.FileName);
    }
}