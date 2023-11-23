using System.Web;

namespace AZ.Integrator.Shared.Infrastructure.UtilityExtensions;

public static class DictionaryExtensions
{
    public static string ToHttpQueryString(this Dictionary<string, string> queryParamsDict)
    {
        var queryString = string.Join("&", queryParamsDict.Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"));

        return queryString;
    }
    
    public static string ToHttpQueryString(this Dictionary<string, object> queryParamsDict)
    {
        var queryString = new List<string>();

        foreach (var kvp in queryParamsDict)
        {
            if (kvp.Value is string strValue)
            {
                queryString.Add($"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(strValue)}");
            }
            else if (kvp.Value is IEnumerable<string> enumerableValues)
            {
                foreach (var value in enumerableValues)
                {
                    queryString.Add($"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(value)}");
                }
            }
        }

        return string.Join("&", queryString);
    }
}