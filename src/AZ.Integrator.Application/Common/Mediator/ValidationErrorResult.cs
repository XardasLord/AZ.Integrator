namespace AZ.Integrator.Application.Common.Mediator;

public sealed record ValidationErrorResult(bool IsValid, IEnumerable<string> Errors);