namespace AZ.Integrator.Infrastructure.UtilityExtensions;

public static class StreamExtensions
{
    public static async Task<byte[]> ReadAsByteArrayAsync(this Stream source)
    {
        if (source is MemoryStream memorySource)
            return memorySource.ToArray();

        using var memoryStream = new MemoryStream();
        await source.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}