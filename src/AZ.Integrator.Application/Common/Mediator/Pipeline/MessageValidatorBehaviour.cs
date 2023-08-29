using Mediator;

namespace AZ.Integrator.Application.Common.Mediator.Pipeline;

public sealed class MessageValidatorBehaviour<TMessage, TResponse> : MessagePreProcessor<TMessage, TResponse>
    where TMessage : IValidate
{
    private readonly IServiceProvider _serviceProvider;

    public MessageValidatorBehaviour(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async ValueTask Handle(TMessage message, CancellationToken cancellationToken)
    {
        var validationResult = await message.IsValid(_serviceProvider);
        
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);
    }
}