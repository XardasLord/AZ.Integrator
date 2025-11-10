using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Tests.Aggregates.Supplier.ValueObjects;

public sealed class SupplierNameTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateSupplierName()
    {
        // Arrange
        const string validName = "Test Supplier Ltd.";

        // Act
        var result = Act(() => SupplierName.Create(validName));

        // Assert
        result.Value.Should().Be(validName);
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimValue()
    {
        // Arrange
        const string nameWithWhitespace = "  Test Supplier  ";
        const string expectedName = "Test Supplier";

        // Act
        var result = Act(() => SupplierName.Create(nameWithWhitespace));

        // Assert
        result.Value.Should().Be(expectedName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidValue_ShouldThrowArgumentException(string invalidName)
    {
        // Act
        var act = () => Act(() => SupplierName.Create(invalidName));

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Supplier name cannot be null or empty*");
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string validName = "Test Supplier";
        var supplierName = SupplierName.Create(validName);

        // Act
        var result = Act(() => supplierName.ToString());

        // Assert
        result.Should().Be(validName);
    }

    [Fact]
    public void ImplicitConversionToString_ShouldReturnValue()
    {
        // Arrange
        const string validName = "Test Supplier";
        var supplierName = SupplierName.Create(validName);

        // Act
        string result = Act(() => (string)supplierName);

        // Assert
        result.Should().Be(validName);
    }

    [Fact]
    public void ImplicitConversionFromString_ShouldCreateSupplierName()
    {
        // Arrange
        const string validName = "Test Supplier";

        // Act
        SupplierName result = Act(() => (SupplierName)validName);

        // Assert
        result.Value.Should().Be(validName);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const string value = "Test Supplier";
        var supplierName1 = SupplierName.Create(value);
        var supplierName2 = SupplierName.Create(value);

        // Act
        var result = Act(() => supplierName1.Equals(supplierName2));

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var supplierName1 = SupplierName.Create("Supplier A");
        var supplierName2 = SupplierName.Create("Supplier B");

        // Act
        var result = Act(() => supplierName1.Equals(supplierName2));

        // Assert
        result.Should().BeFalse();
    }

    private static T Act<T>(Func<T> action) => action();
    private static void Act(Action action) => action();
}

