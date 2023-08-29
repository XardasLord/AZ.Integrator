using Mediator;
using Microsoft.Extensions.Logging;

namespace AZ.Integrator.Application.Common.Mediator.Pipeline;

public sealed class ErrorLoggingBehaviour<TMessage, TResponse> : MessageExceptionHandler<TMessage, TResponse>
    where TMessage : notnull, IMessage
{
    private readonly ILogger<ErrorLoggingBehaviour<TMessage, TResponse>> _logger;

    public ErrorLoggingBehaviour(ILogger<ErrorLoggingBehaviour<TMessage, TResponse>> logger)
    {
        _logger = logger;
    }

    protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(
        TMessage message,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Error handling message of type {messageType}: {@message}", message.GetType().Name, message);
        return NotHandled;
    }
}