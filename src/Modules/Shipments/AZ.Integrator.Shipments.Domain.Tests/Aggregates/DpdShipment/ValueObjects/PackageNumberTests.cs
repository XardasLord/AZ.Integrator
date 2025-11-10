using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment.ValueObjects;

public sealed class PackageNumberTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreatePackageNumber()
    {
        // Arrange
        const long validNumber = 98765L;

        // Act
        var packageNumber = new PackageNumber(validNumber);

        // Assert
        packageNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const long validNumber = 98765L;
        var packageNumber = new PackageNumber(validNumber);

        // Act
        var result = packageNumber.ToString();

        // Assert
        result.Should().Be("98765");
    }

    [Fact]
    public void ImplicitConversionToLong_ShouldReturnValue()
    {
        // Arrange
        const long validNumber = 98765L;
        var packageNumber = new PackageNumber(validNumber);

        // Act
        long result = packageNumber;

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionFromLong_ShouldCreatePackageNumber()
    {
        // Arrange
        const long validNumber = 98765L;

        // Act
        PackageNumber packageNumber = validNumber;

        // Assert
        packageNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const long value = 98765L;
        var packageNumber1 = new PackageNumber(value);
        var packageNumber2 = new PackageNumber(value);

        // Act
        var result = packageNumber1.Equals(packageNumber2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var packageNumber1 = new PackageNumber(98765L);
        var packageNumber2 = new PackageNumber(56789L);

        // Act
        var result = packageNumber1.Equals(packageNumber2);

        // Assert
        result.Should().BeFalse();
    }
}

