﻿using AZ.Integrator.Application.UseCases.Orders.Queries.GetAll;
using AZ.Integrator.Application.UseCases.Orders.Queries.GetDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AllegroOrdersController : ApiBaseController
{
    [HttpGet]
    public async Task<OkObjectResult> GetOrders()
    {
        return Ok(await Mediator.Send(new GetAllQuery()));
    }
    
    [HttpGet("{orderId}")]
    public async Task<OkObjectResult> GetOrderDetails(Guid orderId)
    {
        return Ok(await Mediator.Send(new GetDetailsQuery(orderId)));
    }
}