using AZ.Integrator.Invoices.Application.UseCases.Invoices.Queries.Download;
using AZ.Integrator.Operations.Application.UseCases.Invoices.Commands.GenerateInvoiceForOrder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InvoicesController : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> GenerateInvoice(GenerateInvoiceForOrderCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpGet("{invoiceId}")]
    public async Task<IActionResult> DownloadInvoice(long invoiceId)
    {
        var result = await Mediator.Send(new DownloadQuery(invoiceId));

        return File(result.ContentStream, result.ContentType, result.FileName);
    }
}