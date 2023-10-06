﻿using AZ.Integrator.Application.UseCases.Shipments.Commands.CreateInpostShipment;
using AZ.Integrator.Application.UseCases.Shipments.Queries.GetInpostLabel;
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
    
    [HttpGet("{shipmentId}/label")]
    public async Task<IActionResult> GetShipmentLabel(string shipmentId)
    {
        var result = await Mediator.Send(new GetInpostLabelQuery(shipmentId));

        return File(result.ContentStream, result.ContentType, result.FileName);
    }
}