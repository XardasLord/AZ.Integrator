namespace AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshAccessTokenResponse
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public long expires_in { get; set; }
}