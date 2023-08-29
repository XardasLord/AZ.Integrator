namespace AZ.Integrator.Domain.Extensions;

public static class MiscExtensions
{
    public static bool IsIn<T>(this T source, params T[] list)
    {
        if (source is null) 
            throw new ArgumentNullException(nameof(source));
		
        return list.Contains(source);
    }
}