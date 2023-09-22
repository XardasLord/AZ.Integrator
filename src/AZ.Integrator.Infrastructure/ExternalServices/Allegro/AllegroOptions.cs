namespace AZ.Integrator.Infrastructure.ExternalServices.Allegro;

public class AllegroOptions
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string AuthorizationEndpoint { get; set; }
    public string TokenEndpoint { get; set; }
    public string RedirectUri { get; set; }
    public string ApiUrl { get; set; }
}