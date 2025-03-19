using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.ChangeQuantity;

public record ChangeQuantityCommand(string PackageCode, int ChangeQuantity) : HeaderRequest, IRequest;