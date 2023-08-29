using Mediator;

namespace AZ.Integrator.Application.Common.Mediator;

public interface IValidate : IMessage
{
    Task<ValidationErrorResult> IsValid(IServiceProvider serviceProvider);
}