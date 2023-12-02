using AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;
using AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetDetails;
using AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetOrderProductTags;
using AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetTags;
using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AllegroOrdersController : ApiBaseController
{
    [HttpGet]
    public async Task<OkObjectResult> GetOrders([FromQuery] GetAllQueryFilters filters)
    {
        return Ok(await Mediator.Send(new GetAllQuery(filters)));
    }
    
    [HttpGet("{orderId}")]
    public async Task<OkObjectResult> GetOrderDetails(Guid orderId)
    {
        return Ok(await Mediator.Send(new GetDetailsQuery(orderId)));
    }
    
    [HttpGet("{orderId}/tags")]
    public async Task<OkObjectResult> GetOrderProductTags(Guid orderId)
    {
        return Ok(await Mediator.Send(new GetOrderProductTagsQuery(orderId)));
    }
    
    [HttpGet("tags")]
    public async Task<OkObjectResult> GetTags()
    {
        return Ok(await Mediator.Send(new GetTagsQuery()));
    }
}