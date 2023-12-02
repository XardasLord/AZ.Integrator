namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;

public class AllegroOptions
{
    public OAuthClientSecrets AzTeamTenant { get; set; }
    public OAuthClientSecrets MyTestTenant { get; set; }
    public string AuthorizationEndpoint { get; set; }
    public string TokenEndpoint { get; set; }
    public string RedirectUri { get; set; }
    public string ApiUrl { get; set; }
}

public class OAuthClientSecrets
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUri { get; set; }
}