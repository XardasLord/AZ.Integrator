using AZ.Integrator.Domain.Abstractions;

namespace AZ.Integrator.Shipments.Domain.Tests.TestHelpers;

internal sealed class FakeCurrentDateTime : ICurrentDateTime
{
    public DateTime FixedDateTime { get; set; } = new DateTime(2025, 11, 10, 12, 0, 0);

    public DateTime CurrentDate() => FixedDateTime;
}

