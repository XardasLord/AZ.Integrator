using System.Net.Http.Json;
using System.Text;
using AZ.Integrator.Domain.Abstractions;
using AZ.Integrator.Integrations.Domain.Aggregates.Allegro;
using AZ.Integrator.Integrations.Domain.Aggregates.Allegro.Specifications;
using AZ.Integrator.Shared.Infrastructure.Authentication;
using AZ.Integrator.Shared.Infrastructure.Hangfire.Jobs;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using Mediator;
using Microsoft.IdentityModel.Tokens;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AZ.Integrator.Integrations.Infrastructure.Hangfire.Jobs.RefreshTenantAccessToken;

public class RefreshAllegroTenantAccessTokenCommandHandler(
    IAggregateRepository<AllegroIntegration> repository,
    IHttpClientFactory httpClientFactory,
    AllegroOptions allegroOptions)
    : JobCommandHandlerBase<RefreshAllegroTenantAccessTokenCommand>
{
    public override async ValueTask<Unit> Handle(RefreshAllegroTenantAccessTokenCommand command, CancellationToken cancellationToken)
    {
        await base.Handle(command, cancellationToken);

        var spec = new AllegroIntegrationBySourceSystemIdSpec(command.TenantId, command.SourceSystemId);
        
        var allegroTokenDetails = await repository.SingleOrDefaultAsync(spec, cancellationToken)
            ?? throw new InvalidOperationException($"Allegro integration details not found for Tenant: {command.TenantId}, SourceSystem: {command.SourceSystemId}");

        var queryParams = PrepareQueryFilters(allegroTokenDetails);
        var httpClient = PrepareHttpClient(allegroTokenDetails.ClientId, allegroTokenDetails.ClientSecret);
        
        using var response = await httpClient.PostAsync($"{allegroOptions.TokenEndpoint}?{queryParams}", null, cancellationToken);
        
        response.EnsureSuccessStatusCode();

        var refreshAccessTokenResponse = await response.Content.ReadFromJsonAsync<RefreshAllegroAccessTokenResponse>(cancellationToken: cancellationToken);

        allegroTokenDetails.AccessToken = refreshAccessTokenResponse.access_token;
        allegroTokenDetails.RefreshToken = refreshAccessTokenResponse.refresh_token;
        allegroTokenDetails.ExpiresAt = GetExpiryTimestamp(refreshAccessTokenResponse.access_token);

        await repository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
    
    private static string PrepareQueryFilters(AllegroIntegration allegroIntegration)
    {
        var queryParamsDictionary = new Dictionary<string, object>
        {
            { "grant_type", "refresh_token" }, 
            { "refresh_token", allegroIntegration.RefreshToken }, 
            { "redirect_uri", allegroIntegration.RedirectUri }
        };

        return queryParamsDictionary.ToHttpQueryString();
    }
    
    private HttpClient PrepareHttpClient(string clientId, string secretKey)
    {
        var httpClient = httpClientFactory.CreateClient();
        
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{secretKey}"));
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
        
        return httpClient;
    }
    
    private static DateTime GetExpiryTimestamp(string accessToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return DateTime.MinValue;
            if (!accessToken.Contains("."))
                return DateTime.MinValue;
 
            var parts = accessToken.Split('.');
            var payload = JsonSerializer.Deserialize<JwtToken>(Base64UrlEncoder.Decode(parts[1]));
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(payload.exp);
            
            return dateTimeOffset.LocalDateTime;
        }
        catch (Exception)
        {
            return DateTime.UtcNow.AddHours(12);
        }
    }
}

public class JwtToken
{
    public long exp { get; set; }
}