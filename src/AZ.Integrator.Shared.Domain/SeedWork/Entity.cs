using MediatR;

namespace AZ.Integrator.Domain.SeedWork;

public abstract class Entity
{
    private readonly ISet<INotification> _events = new HashSet<INotification>();

    public IEnumerable<INotification> Events => _events;

    public void AddDomainEvent(INotification @event)
        => _events.Add(@event);

    public void RemoveDomainEvent(INotification @event)
        => _events?.Remove(@event);

    public void ClearDomainEvents()
        => _events?.Clear();
}

public abstract class Entity<T> : Entity
{
    public T Id { get; internal set; }

    private int? _requestedHashCode;

    public bool IsTransient() => Id.Equals(default(T));

    public override bool Equals(object obj)
    {
        if (!(obj is Entity<T>))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var item = (Entity<T>)obj;

        if (item.IsTransient() || IsTransient())
            return false;

        return item.Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        if (IsTransient())
            return base.GetHashCode();

        if (!_requestedHashCode.HasValue)
            _requestedHashCode = Id.GetHashCode() ^ 31;

        return _requestedHashCode.Value;

    }

    public static bool operator ==(Entity<T> left, Entity<T> right)
        => left?.Equals(right) ?? Equals(right, null);

    public static bool operator !=(Entity<T> left, Entity<T> right)
        => !(left == right);
}