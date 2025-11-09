namespace AZ.Integrator.Domain.Extensions;

public static class CorrelationIdHelper
{
    public static string New() => Guid.NewGuid().ToString("N");
}