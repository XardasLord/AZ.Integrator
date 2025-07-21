using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.ChangeStockGroup;

public record ChangeStockGroupCommand(string PackageCode, uint NewGroupId) : HeaderRequest, IRequest;