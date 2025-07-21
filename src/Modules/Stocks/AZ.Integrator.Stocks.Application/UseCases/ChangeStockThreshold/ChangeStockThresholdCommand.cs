using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.ChangeStockThreshold;

public record ChangeStockThresholdCommand(string PackageCode, int Threshold) : HeaderRequest, IRequest;