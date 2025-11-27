namespace AZ.Integrator.Integrations.Application.Common;

public static class SourceSystemIdGenerator
{
    public static string Generate(string prefix)
    {
        var guid = Guid.NewGuid().ToString("N")[..6];
        return $"{prefix}{guid}";
    }
}

