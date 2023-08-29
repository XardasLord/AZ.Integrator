namespace AZ.Integrator.Application.Common.Mediator;

public sealed class ValidationException : Exception
{
    public ValidationException(ValidationErrorResult validationErrorResult) : base("Validation error") =>
        ValidationErrorResult = validationErrorResult;

    public ValidationErrorResult ValidationErrorResult { get; }
}