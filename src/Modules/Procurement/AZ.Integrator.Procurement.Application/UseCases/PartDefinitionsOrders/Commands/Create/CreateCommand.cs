using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using Mediator;

namespace AZ.Integrator.Procurement.Application.UseCases.PartDefinitionsOrders.Commands.Create;

public record CreateCommand(uint SupplierId, IEnumerable<CreateFurnitureLineRequest> FurnitureLineRequests) : ICommand<PartDefinitionsOrderViewModel>;