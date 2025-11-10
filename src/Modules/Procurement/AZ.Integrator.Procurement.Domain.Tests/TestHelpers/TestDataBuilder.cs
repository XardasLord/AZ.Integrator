using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;
using Bogus;

namespace AZ.Integrator.Procurement.Domain.Tests.TestHelpers;

internal static class TestDataBuilder
{
    private static readonly Faker Faker = new();

    public static TenantId CreateTenantId() => new(Faker.Random.Guid());
    
    public static SupplierId CreateSupplierId() => new(Faker.Random.UInt(1, 10000));
    
    public static Email CreateEmail() => new(Faker.Internet.Email());
    
    public static string CreateRandomString(int length = 10) => Faker.Random.AlphaNumeric(length);
    
    public static int CreateRandomInt(int min = 1, int max = 1000) => Faker.Random.Int(min, max);
    public static uint CreateRandomUInt(uint min = 1, uint max = 1000) => Faker.Random.UInt(min, max);
    
    public static Guid CreateRandomGuid() => Faker.Random.Guid();
}

