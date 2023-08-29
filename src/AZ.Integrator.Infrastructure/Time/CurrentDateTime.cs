using AZ.Integrator.Domain.Abstractions;

namespace AZ.Integrator.Infrastructure.Time;

public class CurrentDateTime : ICurrentDateTime
{
    public DateTime CurrentDate() => DateTime.UtcNow;
}