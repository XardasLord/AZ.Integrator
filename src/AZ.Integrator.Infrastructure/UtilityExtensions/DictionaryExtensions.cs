using System.Web;

namespace AZ.Integrator.Infrastructure.UtilityExtensions;

public static class DictionaryExtensions
{
    public static string ToHttpQueryString(this Dictionary<string, string> queryParamsDict)
    {
        var queryString = string.Join("&", queryParamsDict.Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"));

        return queryString;
    }
}