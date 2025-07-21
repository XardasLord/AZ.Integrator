using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.AddStockGroup;

public record AddStockGroupCommand(string Name, string Description) : HeaderRequest, IRequest<uint>;