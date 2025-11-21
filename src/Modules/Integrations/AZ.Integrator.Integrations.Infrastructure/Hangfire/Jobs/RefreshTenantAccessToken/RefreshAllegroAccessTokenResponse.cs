namespace AZ.Integrator.Integrations.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshAllegroAccessTokenResponse
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public long expires_in { get; set; }
}