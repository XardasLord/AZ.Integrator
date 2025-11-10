using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using Bogus;

namespace AZ.Integrator.Shipments.Domain.Tests.TestHelpers;

internal static class TestDataBuilder
{
    private static readonly Faker Faker = new();

    public static TenantId CreateTenantId() => new(Faker.Random.Guid());
    
    public static SourceSystemId CreateSourceSystemId() => new(Faker.Random.AlphaNumeric(10));
    
    public static ExternalOrderNumber CreateExternalOrderNumber() => new(Faker.Random.AlphaNumeric(15));
    
    public static string CreateRandomString(int length = 10) => Faker.Random.AlphaNumeric(length);
    
    public static long CreateRandomLong() => Faker.Random.Long(1, long.MaxValue);
    
    public static Guid CreateRandomGuid() => Faker.Random.Guid();
}

