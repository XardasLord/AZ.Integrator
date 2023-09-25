using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Application.UseCases.Orders.Commands.GetAll;

public record GetAllCommandResponse(IEnumerable<OrderListDto> Orders, int TotalCount);