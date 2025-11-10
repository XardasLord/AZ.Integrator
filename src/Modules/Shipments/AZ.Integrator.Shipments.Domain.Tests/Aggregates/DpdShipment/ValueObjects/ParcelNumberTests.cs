using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment.ValueObjects;

public sealed class ParcelNumberTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateParcelNumber()
    {
        // Arrange
        const long validNumber = 11111L;

        // Act
        var parcelNumber = new ParcelNumber(validNumber);

        // Assert
        parcelNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const long validNumber = 11111L;
        var parcelNumber = new ParcelNumber(validNumber);

        // Act
        var result = parcelNumber.ToString();

        // Assert
        result.Should().Be("11111");
    }

    [Fact]
    public void ImplicitConversionToLong_ShouldReturnValue()
    {
        // Arrange
        const long validNumber = 11111L;
        var parcelNumber = new ParcelNumber(validNumber);

        // Act
        long result = parcelNumber;

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionFromLong_ShouldCreateParcelNumber()
    {
        // Arrange
        const long validNumber = 11111L;

        // Act
        ParcelNumber parcelNumber = validNumber;

        // Assert
        parcelNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const long value = 11111L;
        var parcelNumber1 = new ParcelNumber(value);
        var parcelNumber2 = new ParcelNumber(value);

        // Act
        var result = parcelNumber1.Equals(parcelNumber2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var parcelNumber1 = new ParcelNumber(11111L);
        var parcelNumber2 = new ParcelNumber(22222L);

        // Act
        var result = parcelNumber1.Equals(parcelNumber2);

        // Assert
        result.Should().BeFalse();
    }
}

