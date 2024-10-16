using Mediator;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs;

public abstract class JobCommandHandlerBase<T> : IRequestHandler<T> where T : JobCommandBase
{
	private T _request;

	protected JobCommandHandlerBase() { }

	public virtual ValueTask<Unit> Handle(T command, CancellationToken cancellationToken)
	{
		_request = command;

		return new ValueTask<Unit>();
	}
}