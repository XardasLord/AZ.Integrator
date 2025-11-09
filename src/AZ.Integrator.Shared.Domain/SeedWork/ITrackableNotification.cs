using Mediator;

namespace AZ.Integrator.Domain.SeedWork;

public interface ITrackableNotification : INotification
{
    string CorrelationId { get; }
}