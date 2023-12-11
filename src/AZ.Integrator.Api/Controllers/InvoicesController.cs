using AZ.Integrator.Invoices.Application.UseCases.Invoices.Commands.Register;
using AZ.Integrator.Invoices.Application.UseCases.Invoices.Queries.Download;
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
    
    [HttpGet("{invoiceId}")]
    public async Task<IActionResult> GetShipmentLabel(long invoiceId)
    {
        var result = await Mediator.Send(new DownloadQuery(invoiceId));

        return File(result.ContentStream, result.ContentType, result.FileName);
    }
}