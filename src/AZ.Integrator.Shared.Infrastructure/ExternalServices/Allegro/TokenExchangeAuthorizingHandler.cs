using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AZ.Integrator.Shared.Infrastructure.ExternalServices.Allegro;

public class TokenExchangeAuthorizingHandler : DelegatingHandler
{
    private readonly OAuthOptions _options;
    
    public TokenExchangeAuthorizingHandler(HttpMessageHandler inner, OAuthOptions options)
        : base(inner)
    {
        _options = options;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri == new Uri(_options.TokenEndpoint))
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.ClientId}:{_options.ClientSecret}"));

            request.Headers.Add("Authorization", $"Basic {credentials}");
        }
        
        return base.SendAsync(request, cancellationToken);
    }
}