namespace AZ.Integrator.Shared.Infrastructure.Authentication;

public class AllegroOptions
{
    public string AuthorizationEndpoint { get; set; }
    public string TokenEndpoint { get; set; }
    public string RedirectUri { get; set; }
    public string ApiUrl { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}