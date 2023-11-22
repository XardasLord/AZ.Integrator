using AZ.Integrator.Domain.Abstractions;

namespace AZ.Integrator.Shared.Infrastructure.Time;

public class CurrentDateTime : ICurrentDateTime
{
    public DateTime CurrentDate() => DateTime.UtcNow;
}