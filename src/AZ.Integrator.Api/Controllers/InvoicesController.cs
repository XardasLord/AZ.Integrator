using AZ.Integrator.Application.UseCases.Invoices.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InvoicesController : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> RegisterInvoice(RegisterInvoiceCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}