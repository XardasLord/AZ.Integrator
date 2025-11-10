using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;
using Bogus;
using SupplierAggregate = AZ.Integrator.Procurement.Domain.Aggregates.Supplier.Supplier;

namespace AZ.Integrator.Procurement.Domain.Tests.Aggregates.Supplier;

public sealed class SupplierTests
{
    private readonly FakeCurrentUser _currentUser = new();
    private readonly FakeCurrentDateTime _currentDateTime = new();

    [Fact]
    public void Create_WithValidData_ShouldCreateSupplier()
    {
        // Arrange
        var name = SupplierName.Create("Test Supplier Ltd.");
        var telephoneNumber = TelephoneNumber.Create("+48 123 456 789");
        var mailboxEmails = new List<Email> { new Email("supplier@example.com") };

        // Act
        var result = Act(() => SupplierAggregate.Create(name, telephoneNumber, mailboxEmails, _currentUser, _currentDateTime));

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.TelephoneNumber.Should().Be(telephoneNumber);
        result.TenantId.Value.Should().Be(_currentUser.TenantId);
        result.Mailboxes.Should().HaveCount(1);
        result.Mailboxes.First().Email.Value.Should().Be("supplier@example.com");
    }

    [Fact]
    public void Create_ShouldSetCreationInformationCorrectly()
    {
        // Arrange
        var name = SupplierName.Create("Test Supplier");
        var telephoneNumber = TelephoneNumber.Create("+48 123 456 789");
        var mailboxEmails = new List<Email> { TestDataBuilder.CreateEmail() };
        var fixedDate = new DateTime(2025, 11, 10, 12, 0, 0);
        _currentDateTime.FixedDateTime = fixedDate;

        // Act
        var result = Act(() => SupplierAggregate.Create(name, telephoneNumber, mailboxEmails, _currentUser, _currentDateTime));

        // Assert
        result.CreationInformation.CreatedAt.Should().Be(fixedDate);
        result.CreationInformation.CreatedBy.Should().Be(_currentUser.UserId);
        result.CreationInformation.TenantId.Value.Should().Be(_currentUser.TenantId);
    }

    [Fact]
    public void Create_WithMultipleMailboxes_ShouldCreateAllMailboxes()
    {
        // Arrange
        var name = SupplierName.Create("Multi Mailbox Supplier");
        var telephoneNumber = TelephoneNumber.Create("+48 123 456 789");
        var mailboxEmails = new List<Email>
        {
            new Email("mailbox1@example.com"),
            new Email("mailbox2@example.com"),
            new Email("mailbox3@example.com")
        };

        // Act
        var result = Act(() => SupplierAggregate.Create(name, telephoneNumber, mailboxEmails, _currentUser, _currentDateTime));

        // Assert
        result.Mailboxes.Should().HaveCount(3);
        result.Mailboxes.Select(m => m.Email.Value).Should().Contain(new[]
        {
            "mailbox1@example.com",
            "mailbox2@example.com",
            "mailbox3@example.com"
        });
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateSupplier()
    {
        // Arrange
        var supplier = SupplierAggregate.Create(
            SupplierName.Create("Original Name"),
            TelephoneNumber.Create("+48 111 111 111"),
            new List<Email> { new Email("old@example.com") },
            _currentUser,
            _currentDateTime);

        var newName = SupplierName.Create("Updated Name");
        var newTelephoneNumber = TelephoneNumber.Create("+48 222 222 222");
        var newMailboxEmails = new List<Email> { new Email("new@example.com") };

        // Act
        Act(() => supplier.Update(newName, newTelephoneNumber, newMailboxEmails, _currentUser, _currentDateTime));

        // Assert
        supplier.Name.Should().Be(newName);
        supplier.TelephoneNumber.Should().Be(newTelephoneNumber);
        supplier.Mailboxes.Should().HaveCount(1);
        supplier.Mailboxes.First().Email.Value.Should().Be("new@example.com");
    }

    [Fact]
    public void Update_ShouldUpdateModificationInformation()
    {
        // Arrange
        var supplier = SupplierAggregate.Create(
            SupplierName.Create("Test Supplier"),
            TelephoneNumber.Create("+48 123 456 789"),
            new List<Email> { new Email("test@example.com") },
            _currentUser,
            _currentDateTime);

        var modificationDate = new Faker().Date.Past();
        _currentDateTime.FixedDateTime = modificationDate;

        var newName = SupplierName.Create("Updated Supplier");
        var newTelephoneNumber = TelephoneNumber.Create("+48 987 654 321");
        var newMailboxEmails = new List<Email> { new Email("updated@example.com") };

        // Act
        Act(() => supplier.Update(newName, newTelephoneNumber, newMailboxEmails, _currentUser, _currentDateTime));

        // Assert
        supplier.ModificationInformation.ModifiedAt.Should().Be(modificationDate);
        supplier.ModificationInformation.ModifiedBy.Should().Be(_currentUser.UserId);
    }

    [Fact]
    public void Update_WithEmptyMailboxList_ShouldClearMailboxes()
    {
        // Arrange
        var supplier = SupplierAggregate.Create(
            SupplierName.Create("Test Supplier"),
            TelephoneNumber.Create("+48 123 456 789"),
            new List<Email> { new Email("test@example.com"), new Email("test2@example.com") },
            _currentUser,
            _currentDateTime);

        var newName = SupplierName.Create("Updated Supplier");
        var newTelephoneNumber = TelephoneNumber.Create("+48 987 654 321");
        var emptyMailboxEmails = new List<Email>();

        // Act
        Act(() => supplier.Update(newName, newTelephoneNumber, emptyMailboxEmails, _currentUser, _currentDateTime));

        // Assert
        supplier.Mailboxes.Should().BeEmpty();
    }

    [Fact]
    public void Mailboxes_ShouldBeReadOnly()
    {
        // Arrange
        var name = SupplierName.Create("Test Supplier");
        var telephoneNumber = TelephoneNumber.Create("+48 123 456 789");
        var mailboxEmails = new List<Email> { new Email("test@example.com") };

        // Act
        var result = Act(() => SupplierAggregate.Create(name, telephoneNumber, mailboxEmails, _currentUser, _currentDateTime));

        // Assert
        result.Mailboxes.Should().BeAssignableTo<IReadOnlyCollection<AZ.Integrator.Procurement.Domain.Aggregates.Supplier.SupplierMailbox>>();
    }

    private static T Act<T>(Func<T> action) => action();
    private static void Act(Action action) => action();
}

