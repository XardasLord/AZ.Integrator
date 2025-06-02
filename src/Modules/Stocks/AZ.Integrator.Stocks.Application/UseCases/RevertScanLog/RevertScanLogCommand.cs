using AZ.Integrator.Shared.Application;
using Mediator;

namespace AZ.Integrator.Stocks.Application.UseCases.RevertScanLog;

public record RevertScanLogCommand(string PackageCode, int ScanLogId) : HeaderRequest, IRequest;