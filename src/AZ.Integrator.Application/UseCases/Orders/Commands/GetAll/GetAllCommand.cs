using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Commands.GetAll;

public record GetAllCommand : ICommand<IEnumerable<OrderListDto>>;