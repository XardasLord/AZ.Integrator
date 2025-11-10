using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment.ValueObjects;

public sealed class SessionNumberTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateSessionNumber()
    {
        // Arrange
        const long validNumber = 12345L;

        // Act
        var sessionNumber = new SessionNumber(validNumber);

        // Assert
        sessionNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const long validNumber = 12345L;
        var sessionNumber = new SessionNumber(validNumber);

        // Act
        var result = sessionNumber.ToString();

        // Assert
        result.Should().Be("12345");
    }

    [Fact]
    public void ImplicitConversionToLong_ShouldReturnValue()
    {
        // Arrange
        const long validNumber = 12345L;
        var sessionNumber = new SessionNumber(validNumber);

        // Act
        long result = sessionNumber;

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionFromLong_ShouldCreateSessionNumber()
    {
        // Arrange
        const long validNumber = 12345L;

        // Act
        SessionNumber sessionNumber = validNumber;

        // Assert
        sessionNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const long value = 12345L;
        var sessionNumber1 = new SessionNumber(value);
        var sessionNumber2 = new SessionNumber(value);

        // Act
        var result = sessionNumber1.Equals(sessionNumber2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var sessionNumber1 = new SessionNumber(12345L);
        var sessionNumber2 = new SessionNumber(54321L);

        // Act
        var result = sessionNumber1.Equals(sessionNumber2);

        // Assert
        result.Should().BeFalse();
    }
}

