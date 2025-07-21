using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.UpdateStockGroup;

public record UpdateStockGroupCommand(int GroupId, string Name, string Description) : HeaderRequest, IRequest;