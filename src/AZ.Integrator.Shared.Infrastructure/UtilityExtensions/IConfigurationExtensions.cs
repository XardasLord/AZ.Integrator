using Microsoft.Extensions.Configuration;

namespace AZ.Integrator.Shared.Infrastructure.UtilityExtensions;

public static class IConfigurationExtensions
{
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}