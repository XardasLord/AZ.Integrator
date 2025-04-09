using AZ.Integrator.Stocks.Application.UseCases.ChangeQuantity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AZ.Integrator.Api.Controllers;

// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// public class StocksController : ApiBaseController
// {
//     [HttpPut("{*packageCode}")]
//     public async Task<IActionResult> ChangeStockQuantity(string packageCode, ChangeQuantityCommand command)
//     {
//         command = command with
//         {
//             PackageCode = packageCode
//         };
//         
//         await Mediator.Send(command);
//         
//         return NoContent();
//     }
// }